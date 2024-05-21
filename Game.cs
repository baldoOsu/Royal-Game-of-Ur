using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Button[] skipBtns = { null, null };
        private PictureBox[] numberPBoxes = { null, null };
        private TableLayoutPanel[] diceGrid = { null, null };
        public Game(MainForm form)
        {
            TableLayoutPanel grid = (TableLayoutPanel)form.Controls.Find("board_grid", false)[0];
            diceGrid = new TableLayoutPanel[] { (TableLayoutPanel)form.Controls.Find("dice_panel_orange", false)[0], (TableLayoutPanel)form.Controls.Find("dice_panel_purple", false)[0]};

            Player[] players = { new Player("player1", Color.Orange), new Player("player2", Color.Purple) };
            this.players = players;

            dice = new Dice();
            board = new Board(players, grid, form, this);

            diceBtns = new Button[] {
                (Button)form.Controls.Find("Roll_1", false)[0],
                (Button)form.Controls.Find("Roll_2", false)[0]
            };

            skipBtns = new Button[] {
                (Button)form.Controls.Find("Skip_1", false)[0],
                (Button)form.Controls.Find("Skip_2", false)[0]
            };

            numberPBoxes = new PictureBox[]
            {
                (PictureBox)form.Controls.Find("number_1", false)[0],
                (PictureBox)form.Controls.Find("number_2", false)[0]
            };

            foreach (var diceBtn in diceBtns)
            {
                diceBtn.MouseClick += (object sender, MouseEventArgs e) =>
                {
                    this.RollDice();
                };
            }

            foreach (var btn in skipBtns)
            {
                btn.MouseClick += (object sender, MouseEventArgs e) =>
                {
                    hasPlayerRolledDice = false;
                    Turn();

                    // switch back
                    if ((players[currentPlayerIndex].GetPieces(PieceState.Playing).Count + players[currentPlayerIndex].GetPieces(PieceState.Out).Count) == 0)
                    {
                        Turn();
                    }
                };
            }

            SetupButtons();
        }
        public void RollDice()
        {
            dice.Roll();
            DisplayDice();
            hasPlayerRolledDice = true;
            EnablePieceBtns();
            Console.WriteLine($"Rolled {dice.LastSum}");

            if (dice.LastSum == 0)
            {
                // reset
                hasPlayerRolledDice = false;
                Turn();

                if ((players[currentPlayerIndex].GetPieces(PieceState.Playing).Count + players[currentPlayerIndex].GetPieces(PieceState.Out).Count) == 0)
                {
                    Turn();
                }
            }
        }

        public void MovePiece(Piece piece)
        {
            if (hasPlayerRolledDice == false)
                return;

            bool landedOnRosette = false;
            bool isLegalMove = false;
            if (this.dice.LastSum > 0)
                isLegalMove = board.MovePiece(piece, this.dice.LastSum, out landedOnRosette);

            if (!landedOnRosette && isLegalMove)
            {
                // reset
                hasPlayerRolledDice = false;
                Turn();
            }

            if (landedOnRosette)
            {
                // reset
                hasPlayerRolledDice = false;
                DisablePieceBtns();
                ClearDice();
                diceBtns[currentPlayerIndex].Enabled = true;
                skipBtns[currentPlayerIndex].Enabled = false;
            }

            // if other player has no more pieces left
            if ((players[currentPlayerIndex].GetPieces(PieceState.Playing).Count + players[currentPlayerIndex].GetPieces(PieceState.Out).Count) == 0)
            {
                Turn();
            }
        }

        private void SwitchRollBtn()
        {
            diceBtns[currentPlayerIndex].Enabled = false;
            diceBtns[(currentPlayerIndex + 1) % 2].Enabled = true;

            //skipBtns[currentPlayerIndex].Enabled = false;
            skipBtns[(currentPlayerIndex + 1) % 2].Enabled = false;
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

            skipBtns[currentPlayerIndex].Enabled = false;

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

            skipBtns[currentPlayerIndex].Enabled = true;
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

            foreach (var btn in skipBtns)
            {
                btn.Enabled = false;
            }
        }

        private void DisplayDice()
        {
            ClearDice();
            for (int i = 0; i < 4; i++)
            {
                int diceRoll = dice.LastDice[i];
                string diceStr = diceRoll == 0 ? "empty" : "eye";

                PictureBox pbox = new PictureBox() {
                    Image = Image.FromFile($"../../Assets/Dice/dice_{diceStr}.png"),
                    Size = new Size(60, 60),
                    Margin = new Padding(0,0,0,0)
                };

                this.diceGrid[currentPlayerIndex].Controls.Add(pbox, 0, i);
            }

            numberPBoxes[currentPlayerIndex].Image = Image.FromFile($"../../Assets/Numbers/number_{dice.LastSum}.png");
            numberPBoxes[currentPlayerIndex].Visible = true;
        }

        private void Turn()
        {
            DisablePieceBtns();
            SwitchRollBtn();
            ClearDice();
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            return;
        }

        private void ClearDice()
        {
            this.diceGrid[currentPlayerIndex].Controls.Clear();
            foreach (var pBox in numberPBoxes)
            {
                pBox.Visible = false;
            }
        }
    }
}
