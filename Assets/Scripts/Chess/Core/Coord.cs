using System;



////////////////// ADD COMMENT BY TIN
namespace Chess
{
    public struct Coord : IComparable<Coord>
    {
        public readonly int fileIndex;
        public readonly int rankIndex;

        public Coord(int fileIndex, int rankIndex)
        {
            this.fileIndex = fileIndex;
            this.rankIndex = rankIndex;
        }

        // determine white square or black square
        public bool IsLightSquare()
        {
            return (fileIndex + rankIndex) % 2 != 0;
        }

        //implement Icomparable
        public int CompareTo(Coord other)
        {
            return (fileIndex == other.fileIndex && rankIndex == other.rankIndex) ? 0 : 1;
        }
    }
}
