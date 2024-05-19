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
        private TableLayoutPanel grid;
        private Piece[,] playingPieces = new Piece[3,8];
        
        public Board(Player[] players, TableLayoutPanel grid)
        {
            for (int i = 0; i < 2; i++)
            {
                Player player = players[i];
                for (int j = 0; j < 7; j++)
                {
                    Piece piece = new Piece(player, this, new int[] { 0, 1 });
                    this.playingPieces[i, j] = piece;
                }
            }

            this.grid = grid;
            this.grid.Controls.Add(playingPieces[0, 0], 0, 0);
            this.grid.Controls.Add(playingPieces[0, 1], 0, 1);
            this.grid.Controls.Add(playingPieces[1, 0], 1, 1);
            this.grid.Controls.Add(playingPieces[1, 1], 1, 0);
        }

        public void FocusPiece(Piece piece)
        {
            Console.WriteLine("Focusing piece");
        }
    }
}
