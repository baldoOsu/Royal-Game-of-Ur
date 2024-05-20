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
        Out,
        Playing,
        Finished
    }
    public enum MoveResult
    {
        Move,
        Take
    }

    public class Game
    {
        private Board board;
        private Player[] players;
        private Dice dice;

        private bool hasPlayerRolledDice = false;
        private int currentPlayerIndex = 0;

        private Button[] diceBtns = { null, null };
        public Game(Form1 form)
        {
            TableLayoutPanel grid = (TableLayoutPanel)form.Controls.Find("board_grid", false)[0];

            Player[] players = { new Player("player1", Color.Orange), new Player("player2", Color.Purple) };
            this.players = players;

            dice = new Dice();
            board = new Board(players, grid, form, this);

            diceBtns[0] = (Button)form.Controls.Find("Roll_1", false)[0];
            diceBtns[1] = (Button)form.Controls.Find("Roll_2", false)[0];

            foreach (var diceBtn in diceBtns)
            {
                diceBtn.MouseClick += (object sender, MouseEventArgs e) =>
                {
                    this.RollDice();
                };
            }

            SetupButtons();
        }

        public void MovePiece(Piece piece)
        {
            if (hasPlayerRolledDice == false)
                return;

            bool landedOnRosette = false;
            bool isLegalMove = false;
            if (this.dice.Last > 0)
                isLegalMove = board.MovePiece(piece, this.dice.Last, out landedOnRosette);

            if (!landedOnRosette && isLegalMove)
            {
                // reset
                hasPlayerRolledDice = false;
                DisablePieceBtns();
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }

            if (landedOnRosette)
            {
            }
        }

        public void RollDice()
        {
            dice.Roll();
            hasPlayerRolledDice = true;
            EnablePieceBtns();
            Console.WriteLine($"Rolled {dice.Last}");

            if (dice.Last == 0)
            {
                // reset
                hasPlayerRolledDice = false;
                DisablePieceBtns();
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
        }

        private void DisablePieceBtns()
        {
            Player player = players[currentPlayerIndex];

            foreach (var piece in player.GetPieces(PieceState.Out))
            {
                piece.Enabled = false;
            }
            foreach (var piece in player.GetPieces(PieceState.Playing))
            {
                piece.Enabled = false;
            }

            diceBtns[currentPlayerIndex].Enabled = false;
            diceBtns[(currentPlayerIndex + 1) % 2].Enabled = true;
        }
        private void EnablePieceBtns()
        {
            Player player = players[currentPlayerIndex];

            foreach (var piece in player.GetPieces(PieceState.Out))
            {
                piece.Enabled = true;
            }
            foreach (var piece in player.GetPieces(PieceState.Playing))
            {
                piece.Enabled = true;
            }

            foreach (var Dice in diceBtns)
            {
                Dice.Enabled = false;
            }
        }

        private void SetupButtons()
        {
            foreach (var player in players)
            {
                foreach (var piece in player.GetPieces(PieceState.Out))
                {
                    piece.Enabled = false;
                }
                foreach (var piece in player.GetPieces(PieceState.Playing))
                {
                    piece.Enabled = false;
                }
            }
            diceBtns[(currentPlayerIndex + 1) % 2].Enabled = false;
        }
    }
}
