﻿using Asterion.Core;
using Asterion.Input;
using Asterion.UI;
using Asterion.UI.Controls;

namespace Asterion.Demo.UIPages
{
    public sealed class PageGameWorld : UIPage
    {
        private static readonly Position BOARD_POSITION = new Position(0, 0);
        private static readonly Dimension BOARD_SIZE = new Dimension(48, 20);

        private static readonly string[] MAP = new string[]
        {
            "................................................",
            "................................................",
            "................................................",
            "................................................",
            "................................................",
            "...................................www..........",
            "..................................wwwww.........",
            ".............WWWWWWW..............wwwww.........",
            ".............WfffffW..............wwwww.........",
            ".............WfffffW...............www..........",
            ".............WfffffW................ww..........",
            ".............WWWDWWW.................ww.........",
            "......................................ww........",
            "..T.....T..............................www......",
            ".....T...................................ww.....",
            "...T.TT...T...............................wwww..",
            ".T.TT..T....................................wwww",
            ".TT..T..T....T..................................",
            "TTT.T..T...T....................................",
            "TT..TTT........................................."
        };

        private UITileBoard TileBoard;

        private bool AttackMode = false;
        private bool DoorOpen = false;

        private Position PlayerPosition = new Position(3, 3);

        protected override void OnInitialize(object[] parameters)
        {
            UI.Cursor.Enabled = false;
            UI.Cursor.Moveable = true;
            UI.Cursor.BoundingBox = new Area(BOARD_POSITION, BOARD_SIZE);
            UI.Cursor.Tile = (int)TileID.CursorCrosshair;
            UI.Cursor.VFX = TileVFX.GlowFast;

            TileBoard = AddTileBoard(BOARD_POSITION.X, BOARD_POSITION.Y, BOARD_SIZE.Width, BOARD_SIZE.Height);

            AddImage(0, BOARD_POSITION.Y + BOARD_SIZE.Height, 48, 1, (int)TileID.Frame + 4, RGBColor.CornflowerBlue);
            AddLabel(2, UI.Game.Renderer.TileCount.Height - 4, "Arrow keys, gamepad sticks/DPad: move cursor", (int)TileID.Font, RGBColor.PaleGoldenrod);
            AddLabel(2, UI.Game.Renderer.TileCount.Height - 3, "F: fullscreen toggle, ESC: back", (int)TileID.Font, RGBColor.PaleGoldenrod);

            UpdateWorld();
        }

        private void UpdateWorld()
        {
            TileBoard.Clear(new UITileBoardTile((int)TileID.Grass, RGBColor.DarkOliveGreen));

            int x, y;
            for (y = 0; y < TileBoard.BoardSize.Height; y++)
                for (x = 0; x < TileBoard.BoardSize.Width; x++)
                {
                    switch (MAP[y][x])
                    {
                        case 'D': TileBoard[x, y] = new UITileBoardTile((int)(DoorOpen ? TileID.DoorOpen : TileID.DoorClosed), RGBColor.SaddleBrown); break; // Door
                        case 'f': TileBoard[x, y] = new UITileBoardTile((int)TileID.Wall, new RGBColor(64), 0, TileVFX.Negative); break; // Floorboards
                        case 'T': TileBoard[x, y] = new UITileBoardTile((int)TileID.Tree, RGBColor.ForestGreen, 0, TileVFX.OscillateTopSlow); break; // Tree
                        case 'w': TileBoard[x, y] = new UITileBoardTile((int)TileID.Grass, RGBColor.Blue, 0, TileVFX.WaveHorizontalMedium); break; // Water
                        case 'W': TileBoard[x, y] = new UITileBoardTile((int)TileID.Wall, RGBColor.Gray); break; // Wall
                        default: TileBoard[x, y] = new UITileBoardTile((int)TileID.Grass, RGBColor.DarkOliveGreen); break; // Grass
                    }
                }

            TileBoard[PlayerPosition] = new UITileBoardTile((int)TileID.Skeleton, RGBColor.AntiqueWhite);
        }

        private void MovePlayer(Position direction)
        {
            if (AttackMode) return;

            Position newPosition = PlayerPosition + direction;

            if (
                (!TileBoard.BoardSize.Contains(newPosition)) || // Out of bounds
                (MAP[newPosition.Y][newPosition.X] == 'W') || // Tile is a wall
                (MAP[newPosition.Y][newPosition.X] == 'T') || // Tile is a tree
                ((MAP[newPosition.Y][newPosition.X] == 'D') && !DoorOpen)  // Tile is a closed door
                )
            {
                if (MAP[newPosition.Y][newPosition.X] == 'D') // Bumped into the closed door, open it
                {
                    DoorOpen = true;
                    UpdateWorld();
                }

                UI.Game.Audio.PlaySound("noway.wav");
                return;
            }

            UI.Game.Audio.PlaySound("walk.wav");
            PlayerPosition = newPosition;
            UpdateWorld();
        }

        protected override void OnInputEvent(KeyCode key, ModifierKeys modifiers, int gamepadIndex, bool isRepeat)
        {
            if (modifiers != 0) return;

            switch (key)
            {
                case KeyCode.Up:
                    MovePlayer(-Position.OneY); return;

                case KeyCode.Down:
                    MovePlayer(Position.OneY); return;

                case KeyCode.Left:
                    MovePlayer(-Position.OneX); return;

                case KeyCode.Right:
                    MovePlayer(Position.OneX); return;

                case KeyCode.Escape:
                    UI.ShowPage<PageMainMenu>(); return;

                case KeyCode.Space:
                    if (AttackMode)
                    {
                        AttackMode = false;
                        UI.Cursor.Enabled = false;
                    }
                    else
                    {
                        AttackMode = true;
                        UI.Cursor.Enabled = true;
                        UI.Cursor.Position = PlayerPosition + BOARD_POSITION;
                    }
                    return;
            }
        }

        protected override void OnClose()
        {
            UI.Cursor.Enabled = false;
        }
    }
}