using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Royal_Game_of_Ur
{
    public class Player
    {
        private List<Piece> newPieces = new List<Piece>();
        private List<Piece> outPieces = new List<Piece>();
        private List<Piece> finishedPieces = new List<Piece>();

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

        public List<Piece> GetNewPieces()
        {
            return this.newPieces;
        }
        public List<Piece> GetOutPieces()
        {
            return this.outPieces;
        }
        public List<Piece> GetFinishedPieces()
        {
            return this.finishedPieces;
        }
        public void AddNewPiece(Piece piece)
        {
            this.newPieces.Add(piece);
        }
        public void RemoveNewPiece(Piece piece)
        {
            for (int i = 0; i < 7; i++)
            {
                this.newPieces.Remove(piece);
            }
        }
        public void AddOutPiece(Piece piece)
        {
            this.outPieces.Add(piece);
        }
        public void RemoveOutPiece(Piece piece)
        {
            this.finishedPieces.Remove(piece);
        }
        public void AddFinishedPiece(Piece piece)
        {
            this.finishedPieces.Add(piece);
        }
        public void ClearPieces()
        {
            this.newPieces.Clear();
            this.outPieces.Clear();
            this.finishedPieces.Clear();
        }
    }
}
