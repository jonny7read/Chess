using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Shell
{

    public class Chess 
    {
        private List<Piece> pieceList;
        private bool checkMate;
        private bool whiteTurn;
        
        public static void Main() 
        {
            var chess = new Chess();
            chess.Init();
            chess.Run();
        }
        
        public void Init()
        {
            InitPieces();
            whiteTurn = true;
        }
        
        public void Run()
        {
            while (!checkMate)
            {
                bool validMove = false;
                Piece piece = null;
                int x = 0, y = 0;
                string error = null;
                
                Console.Clear();
                Console.WriteLine((whiteTurn ? "White" : "Black") + "'s turn");
            
                while (!validMove)
                {
                    Console.Clear();
                    PrintBoard();
                    PrintError(error);
                    Console.WriteLine("Enter your move (<piece> <x> <y>):");
                    string move = Console.ReadLine();
            
                    string[] moveParts = move.Split(null);
                    if (moveParts[0].Equals("exit"))
                    {
                        return;
                    }
                    else if (moveParts.Length < 3) 
                    {
                        error = "Invalid move, make sure you supply all 3 attributes";
                        continue;
                    }
            
                    piece = GetPiece(moveParts[0]);
                    int.TryParse(moveParts[1], out x);
                    int.TryParse(moveParts[2], out y);
            
                    error = ValidateMove(piece, x, y);
                    validMove = error == null;
                }
            
                MovePiece(piece, x-1, y-1);
                Console.WriteLine($"{piece.GetActualName()} moved to {x}, {y}?");
                whiteTurn = !whiteTurn;
            }
        }
        
        private void PrintError(string error)
        {
            if (error == null) return;
            
            Console.WriteLine(error);
        }
        
        private string ValidateMove(Piece piece, int x, int y)
        {
            if (piece == null)
            {
                return "Invalid piece";
            }
            else if (x < 1 || x > 8)
            {
                return "Invalid x coordinate";
            }
            else if (y < 1 || y > 8)
            {
                return "Invalid y coordinate";
            }
            else if (whiteTurn != piece.IsWhite())
            {
                return "That's not your piece!";
            }
            else if (!piece.CanMove(x-1, y-1)) {
                return "Can't move there!";
            }
            else
            {
                return null;
            }
        }
        
        private void InitPieces()
        {
            pieceList = new List<Piece>();
            var color = ConsoleColor.Black;
            pieceList.Add(new Piece(PieceType.ROOK, color, 0, 0, 1));
            pieceList.Add(new Piece(PieceType.KNIGHT, color, 1, 0, 1));
            pieceList.Add(new Piece(PieceType.BISHOP, color, 2, 0, 1));
            pieceList.Add(new Piece(PieceType.KING, color, 3, 0));
            pieceList.Add(new Piece(PieceType.QUEEN, color, 4, 0));
            pieceList.Add(new Piece(PieceType.BISHOP, color, 5, 0, 2));
            pieceList.Add(new Piece(PieceType.KNIGHT, color, 6, 0, 2));
            pieceList.Add(new Piece(PieceType.ROOK, color, 7, 0, 2));
            for (int x = 0; x < 8; x++)
            {
                pieceList.Add(new Piece(PieceType.PAWN, color, x, 1, x+1));
            }
            
            color = ConsoleColor.White;
            for (int x = 0; x < 8; x++)
            {
                pieceList.Add(new Piece(PieceType.PAWN, color, x, 6, x+1));
            }
            pieceList.Add(new Piece(PieceType.ROOK, color, 0, 7, 1));
            pieceList.Add(new Piece(PieceType.KNIGHT, color, 1, 7, 1));
            pieceList.Add(new Piece(PieceType.BISHOP, color, 2, 7, 1));
            pieceList.Add(new Piece(PieceType.KING, color, 3, 7));
            pieceList.Add(new Piece(PieceType.QUEEN, color, 4, 7));
            pieceList.Add(new Piece(PieceType.BISHOP, color, 5, 7, 2));
            pieceList.Add(new Piece(PieceType.KNIGHT, color, 6, 7, 2));
            pieceList.Add(new Piece(PieceType.ROOK, color, 7, 7, 2));
        }
        
        private void PrintBoard()
        {
            Console.WriteLine("  |1  |2  |3  |4  |5  |6  |7  |8");
            for (int y = 0; y < 8; y++)
            {
                Console.WriteLine("-----------------------------------");
                Console.Write(" {0}|", y + 1);
                StringBuilder rowText = new StringBuilder();
                for (int x = 0; x < 8; x++)
                {
                    Piece piece = null;
                    foreach (var p in pieceList)
                    {
                        if (p.X == x && p.Y == y)
                        {
                            piece = p;
                            break;
                        }
                    }
                    
                    if (piece != null)
                    {
                        rowText.Append(piece.ToString());
                    }
                    else
                    {
                        rowText.Append("   ");
                    }
                    rowText.Append("|");
                }
                
                Console.WriteLine(rowText.ToString());
            }
            Console.WriteLine("------------------------------------");
            Console.WriteLine();
        }
        
        private Piece GetPiece(string pieceCode)
        {
            if (pieceCode == null || pieceCode.Length < 3) return null;
            
            string colorCode = pieceCode.Substring(0,1);
            string typeCode = pieceCode.Substring(1, 1);
            int sequenceNumber = 0;
            int.TryParse(pieceCode.Substring(2, 1), out sequenceNumber);
            if (sequenceNumber == 0) return null;
            
            foreach (Piece piece in pieceList)
            {
                if (!piece.GetTypeCode().ToLower().Equals(typeCode.ToLower()))
                {
                    continue;
                }
                else if (!piece.GetColorCode().Equals(colorCode))
                {
                    continue;
                }
                else if (piece.SequenceNumber != sequenceNumber)
                {
                    continue;
                }
                else
                {
                    return piece;
                }
            }
            return null;
        }
        
        private void MovePiece(Piece piece, int x, int y)
        {
            piece.SetLocation(x, y);
        }
    }
}
