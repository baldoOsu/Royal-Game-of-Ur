using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Royal_Game_of_Ur
{
    public enum Color
    {
        Orange,
        Purple
    }
    public enum PieceState
    {
        New,
        Playing,
        Out,
        Finished
    }
    public enum MoveResult
    {
        Move,
        Take
    }

    public class Game
    {
        public Game(Form1 form)
        {
            TableLayoutPanel grid = (TableLayoutPanel)form.Controls.Find("board_grid", false)[0];
            Player[] players = { new Player("player1", Color.Purple), new Player("player2", Color.Orange) };
            Board board = new Board(players, grid);
        }
    }
}
