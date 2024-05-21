using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Royal_Game_of_Ur
{
    public class Board
    {
        private readonly TableLayoutPanel playingGrid;
        private readonly TableLayoutPanel[] outGrids;
        private readonly TableLayoutPanel[] finishedGrids;

        private readonly Piece[,] playingPieces = new Piece[3,8];
        private readonly List<int[]> rosettePositions = new List<int[]>
        {
            new int[] { 0, 0 },
            new int[] { 2, 0 },
            new int[] { 1, 3 },
            new int[] { 0, 6 },
            new int[] { 2, 6 }
        };

        public Board(Player[] players, TableLayoutPanel grid, MainForm form, Game game)
        {
            outGrids = new TableLayoutPanel[] { (TableLayoutPanel)form.Controls.Find("panel_out_orange", true)[0], (TableLayoutPanel)form.Controls.Find("panel_out_purple", true)[0] };
            finishedGrids = new TableLayoutPanel[] { (TableLayoutPanel)form.Controls.Find("panel_finished_orange", true)[0], (TableLayoutPanel)form.Controls.Find("panel_finished_purple", true)[0] };


            for (int i = 0; i < 2; i++)
            {
                Player player = players[i];
                for (int j = 0; j < 7; j++)
                {
                    Piece piece = new Piece(player, this, new int[] { 0, j });
                    piece.MouseClick += (object sender, MouseEventArgs e) =>
                    {
                        game.MovePiece(piece);
                    };

                    outGrids[i].Controls.Add(piece);
                }
            }

            this.playingGrid = grid;
        }

        public bool IsValidMove(Piece piece, int[] newPosition)
        {
            // Check if the new position is within the board's boundaries
            if (newPosition[0] < 0 || newPosition[0] > 2 || newPosition[1] < 0 || newPosition[1] > 7)
            {
                // Shouldn't happen
                Console.WriteLine("Out of bounds!");
                return false;
            }

            // Make sure they can't go further than finish tile
            if (piece.State == PieceState.Playing && piece.BoardPosition[1] > 5 && newPosition[1] < 5)
                return false;

            // 🐈
            if (playingPieces[newPosition[0], newPosition[1]] != null)
            {
                // player can't take own piece
                if (playingPieces[newPosition[0], newPosition[1]].Player == piece.Player)
                    return false;

                // player can't take on rosette
                if (IsRosette(newPosition))
                    return false;
            }

            return true;
        }

        // 🌹
        public bool IsRosette(int[] position)
        {
            return rosettePositions.Any(p => p[0] == position[0] && p[1] == position[1]);
        }

        // returns if landed on rosette
        public bool MovePiece(Piece piece, int moveBy, out bool landedOnRosette)
        {
            int[] oldPosition = piece.BoardPosition;
            int[] newPosition = piece.GetPossibleMovePosition(moveBy);

            if (IsValidMove(piece, newPosition))
            {
                CapturePiece(piece, newPosition);
                piece.SetPosition(newPosition);

                if (piece.State == PieceState.Out)
                    piece.SetState(PieceState.Playing);
                else
                    playingPieces[oldPosition[0], oldPosition[1]] = null;

                int[] finishTile = new int[] { (int)piece.Player.color * 2, 5 };

                if (newPosition[0] != finishTile[0] || newPosition[1] != finishTile[1])
                    playingPieces[newPosition[0], newPosition[1]] = piece;

                this.playingGrid.Controls.Remove(piece);

                if (newPosition[0] != 1 && newPosition[1] == 5)
                {
                    int howManyFinished = this.finishedGrids[(int)piece.Player.color].Controls.Count;

                    this.finishedGrids[(int)piece.Player.color].Controls.Add(piece, 0, howManyFinished);
                    piece.SetState(PieceState.Finished);
                    piece.Enabled = false;
                    landedOnRosette = false;
                }
                else
                {
                    this.playingGrid.Controls.Add(piece, newPosition[0], newPosition[1]);
                    landedOnRosette = IsRosette(newPosition);
                }

                return true;
            }

            landedOnRosette = false;
            return false;
        }

        public void CapturePiece(Piece piece, int[] newPosition)
        {
            Piece targetPiece = playingPieces[newPosition[0], newPosition[1]];
            if (targetPiece != null && targetPiece.Player != piece.Player)
            {
                targetPiece.SetPosition(new int[] { 0, 1 }); // Reset position
                targetPiece.SetState(PieceState.Out);
                this.playingGrid.Controls.Remove(targetPiece);
                this.outGrids[(int)targetPiece.Player.color].Controls.Add(targetPiece);
            }
        }
    }
}
