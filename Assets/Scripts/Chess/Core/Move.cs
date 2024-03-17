/* 
To preserve memory during search, moves are stored as 16 bit numbers.
The format is as follows:

bit 0-5: from square (0 to 63)
bit 6-11: to square (0 to 63)
bit 12-15: flag
*/


//  COMMENT BY TIN 


namespace Chess
{

    public readonly struct Move
    {
        
        public readonly struct Flag
        {
            public const int None = 0;
            public const int EnPassantCapture = 1;
            public const int Castling = 2;
            public const int PromoteToQueen = 3;
            public const int PromoteToKnight = 4;
            public const int PromoteToRook = 5;
            public const int PromoteToBishop = 6;
            public const int PawnTwoForward = 7;
        }

        readonly ushort moveValue;

        const ushort startSquareMask = 0b0000000000111111; // 6 last bit (0-5th) for start square pos (0-63) (2^6=64)
        const ushort targetSquareMask = 0b0000111111000000; // 6 bit (6-11th) for end square pos (0-63)
        const ushort flagMask = 0b1111000000000000; // 4 first bit (12-15th) for flag

        public Move(ushort moveValue)
        {
            this.moveValue = moveValue;
        }


        public Move(int startSquare, int targetSquare) // combine  target , start by bit operator to make the move (ushort)
        {
            moveValue = (ushort)(startSquare | targetSquare << 6);
        }

        public Move(int startSquare, int targetSquare, int flag) // combine  target , start , flag by bit operator to make the move (ushort)
        {
            moveValue = (ushort)(startSquare | targetSquare << 6 | flag << 12);
        }

        public int StartSquare // get start square pos (0-63) by use AND bitwise operator
        {
            get
            {
                return moveValue & startSquareMask;
            }
        }

        public int TargetSquare // get end square pos (0-63) by use AND bitwise operator
        {
            get
            {
                return (moveValue & targetSquareMask) >> 6;
            }
        }

        public bool IsPromotion // check flag = promoted ?? (queen , rook , knight , bishop)
        {
            get
            {
                int flag = MoveFlag;
                return flag == Flag.PromoteToQueen || flag == Flag.PromoteToRook || flag == Flag.PromoteToKnight || flag == Flag.PromoteToBishop;
            }
        }

        public int MoveFlag // get flag (0-63) by use  bitwise operator
        {
            get
            {
                return moveValue >> 12;
            }
        }

        public int PromotionPieceType // promote (queen , rook , knight , bishop)
        {
            get
            {
                switch (MoveFlag)
                {
                    case Flag.PromoteToRook:
                        return Piece.Rook;
                    case Flag.PromoteToKnight:
                        return Piece.Knight;
                    case Flag.PromoteToBishop:
                        return Piece.Bishop;
                    case Flag.PromoteToQueen:
                        return Piece.Queen;
                    default:
                        return Piece.None;
                }
            }
        }

        public static Move InvalidMove // Invalid Move=0
        {
            get
            {
                return new Move(0);
            }
        }

        public static bool SameMove(Move a, Move b)
        {
            return a.moveValue == b.moveValue;
        }

        public ushort Value // Property for moveValue (only get)
        {
            get
            {
                return moveValue;
            }
        }

        public bool IsInvalid //when assign IsInvalid (bool) , moveValue=0 
        {
            get
            {
                return moveValue == 0;
            }
        }

        public string Name //string name of start square and target square (" a1 , b2 ,.. ")
        {
            get
            {
                return BoardRepresentation.SquareNameFromIndex(StartSquare) + "-" + BoardRepresentation.SquareNameFromIndex(TargetSquare);
            }
        }
    }
}


