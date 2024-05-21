using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Royal_Game_of_Ur
{
    public class Piece : Button
    {
        public int[] BoardPosition
        { get; private set; }

        public readonly Player Player;
        public readonly Board Board;
        public PieceState State
        { get; private set; } = PieceState.Out;

        public Piece(Player player, Board board, int[] position)
        {
            this.Height = 69;
            this.Width = 69;

            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.BorderSize = 0;
            
            this.Image = Image.FromFile($"../../Assets/Pieces/piece_{player.color}.png");
            this.Margin = new Padding(0, 2, 0, 0);

            this.BoardPosition = position;
            this.Player = player;
            this.Board = board;

            this.MouseHover += new EventHandler(OnHover);
            this.MouseLeave+= new EventHandler(OnLeave);

            this.Player.AddPiece(this);
        }
        public void SetPosition(int[] position)
        {
            if (!IsPositionInValidRange(position))
                throw new ArgumentOutOfRangeException("position");

            this.BoardPosition = position;
        }
        private bool IsPositionInValidRange(int[] position)
        {
            return !(position[0] < 0 || position[0] > 2 || position[1] < 0 || position[1] > 7);
        }

        public void SetState(PieceState state)
        {
            this.State = state;
        }
        private void OnHover(object sender, EventArgs e)
        {

            Cursor = Cursors.Hand;
        }
        private void OnLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        public int[] GetPossibleMovePosition(int moveBy)
        {
            int X = this.BoardPosition[0];
            int Y = this.BoardPosition[1];

            // can't move finished pieces
            if (moveBy == 0 || this.State == PieceState.Finished) return null;

            // move from spawn into board
            if (this.State == PieceState.Out)
            {
                return new int[] { (int)Player.color * 2, 4 - moveBy };
            }

            if (X != 1)
            {
                // move up in spawn row
                if (Y - moveBy > -1 || Y > 4)
                    return new int[] { (int)Player.color * 2, Y - moveBy };

                // move from spawn tiles into middle row
                return new int[] { 1, -1 - (Y - moveBy) };
            }

            // move down middle row
            if (Y + moveBy < 8)
                return new int[] { 1, Y + moveBy };

            // move from middle row to end tiles
            return new int[] { (int)Player.color * 2, 8 - ((moveBy + Y) % 7) };
        }
    }
}
