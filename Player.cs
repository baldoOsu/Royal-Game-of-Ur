using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Royal_Game_of_Ur
{
    public class Player
    {
        private List<Piece> pieces = new List<Piece>();

        public string Name { get; private set; }
        public int Score { get; private set; }
        public readonly Royal_Game_of_Ur.Color color;

        public Player(string name, Color color)
        {
            this.Score = 0;

            this.Name = name;
            this.color = color;
        }
        public void IncrementScore()
        {
            this.Score += 1;
        }

        public List<Piece> GetPieces()
        {
            return pieces;
        }
        public List<Piece> GetPieces(PieceState state)
        {
            return pieces.FindAll(p => p.State == state);
        }
        public void AddPiece(Piece piece)
        {
            pieces.Add(piece);
        }
        public void RemovePiece(Piece piece)
        {
            pieces.Remove(piece);
        }


        public void ClearPieces()
        {
            this.pieces.Clear();
        }
    }
}
