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
        private int[] boardPosition;

        public readonly Player player;
        public readonly Board board;
        private PieceState state;

        public Piece(Player player, Board board, int[] position)
        {
            this.Height = 69;
            this.Width = 69;

            this.Padding = new Padding(5, 5, 0, 0);

            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.BorderSize = 0;
            
            this.Image = Image.FromFile($"../../Assets/piece_{player.color}.png");

            this.boardPosition = position;
            this.player = player;
            this.board = board;
            this.state = PieceState.New;

            this.Click += new EventHandler(OnClick);

            this.player.AddNewPiece(this);
        }
        public int[] GetPosition()
        {
            return this.boardPosition;
        }
        public void SetPosition(int[] position)
        {
            if (!IsPositionInValidRange(position))
                throw new ArgumentOutOfRangeException("position");

            this.boardPosition = position;
        }
        public PieceState GetState()
        {
            return this.state;
        }
        public void SetState(PieceState state)
        {
            this.state = state;
        }
        private bool IsPositionInValidRange(int[] position)
        {
            return !(position[0] < 0 || position[0] > 2 || position[1] < 0 || position[1] > 7);
        }

        protected void OnClick(object sender, EventArgs e)
        {
            board.FocusPiece(this);
        }
    }
}
