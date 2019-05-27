using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CSharp_Shell
{

    public class Piece
    {
        private PieceType Type { get; }
        private ConsoleColor Color {get;}
        public int X {get;set;}
        public int Y {get;set;}
        public int SequenceNumber {get;}
        private bool hasMoved;
        
        public Piece(PieceType type, ConsoleColor color, int x, int y, int sequenceNumber)
        {
            this.Type = type;
            this.Color = color;
            this.X = x;
            this.Y = y;
            this.SequenceNumber = sequenceNumber;
            this.hasMoved = false;
        }
        
        public Piece(PieceType type, ConsoleColor color, int x, int y) : this(type, color, x, y, 1) {}
        
        public string GetColorCode()
        {
            return Color.ToString().Substring(0,1);
        }
        
        public bool IsWhite()
        {
            return GetColorCode().Equals("W");
        }
        
        public string GetCode()
        {
            string typeLetter = GetTypeCode();
            string colorLetter = GetColorCode();
            return colorLetter + typeLetter + SequenceNumber;
        }
        
        public string GetActualName()
        {
            return this.Color.ToString() + " " + this.Type.ToString().ToLower();
        }
        
        override public string ToString()
        {
            return GetCode();
        }
    
        public PieceType parseType(string code)
        {
            if (code == null) 
            {
                throw new ArgumentNullException();
            }
        
            switch (code)
            {
                case "K":
                    return PieceType.KING;
                case "Q":
                    return PieceType.QUEEN;
                case "B":
                    return PieceType.BISHOP;
                case "H":
                    return PieceType.KNIGHT;
                case "R":
                    return PieceType.ROOK;
                case "P":
                    return PieceType.PAWN;
                default:
                    throw new ArgumentException();
            }
        }
    
        public string GetTypeCode()
        {
            switch (Type)
            {
                case PieceType.KING:
                    return "K";
                case PieceType.QUEEN:
                    return "Q";
                case PieceType.BISHOP:
                    return "B";
                case PieceType.KNIGHT:
                    return "H";
                case PieceType.ROOK:
                    return "R";
                case PieceType.PAWN:
                    return "P";
                default:
                    return null;
            }
        }
        
        public void SetLocation(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.hasMoved = false;
        }
        
        public bool CanMove(int x, int y)
        {
            int xDiff = Math.Abs(x - X);
            int yDiff = Math.Abs(y - Y);
            
            if (false)
            {
                Console.WriteLine(x + " " + y);
                Console.WriteLine(X + " " + Y);
                Console.WriteLine(xDiff + " " + yDiff);
            }
            
            switch (Type)
            {
                case PieceType.KING:
                    return xDiff <= 1 && yDiff <= 1;
                case PieceType.QUEEN:
                    return xDiff == yDiff || xDiff == 0 || yDiff == 0;
                case PieceType.BISHOP:
                    return xDiff == yDiff;
                case PieceType.KNIGHT:
                    return (xDiff == 2 && yDiff == 1) || (xDiff == 1 && yDiff == 2);
                case PieceType.ROOK:
                    return xDiff == 0 || yDiff == 0;
                case PieceType.PAWN:
                    return x == X && y > Y && (hasMoved ? (yDiff) == 1 : yDiff <= 2);
                default:
                    return false;
            }
        }
        
    }
}
