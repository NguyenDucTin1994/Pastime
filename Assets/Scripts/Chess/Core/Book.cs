

////////////////// ADD COMMENT BY TIN

namespace Chess
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

    [System.Serializable]
    public class Book
    {

        public Dictionary<ulong, BookPosition> bookPositions; // Dictionary has  a key= chess case (ulong) , and value (bookPosition) has a dictionary (numTimesMovePlayed) has key = move (ushort)
                                                              // with value is number player use (int)

        public Book()
        {
            bookPositions = new Dictionary<ulong, BookPosition>();
        }

        public bool HasPosition(ulong positionKey) // check for have Key (positionKey) in Dictionary (bookPositions)
        {
            return bookPositions.ContainsKey(positionKey);
        }

        public BookPosition GetBookPosition(ulong key) // get Value
        {
            return bookPositions[key];
        }

        public Move GetRandomBookMove(ulong key) // get random move for a chess case
        {
            var p = bookPositions[key]; // get value ( is an other Dictionay<ushort , int> BookPosition.numTimesMovePlayed)

            ushort[] moves = new List<ushort>(p.numTimesMovePlayed.Keys).ToArray(); // array of ushort (ushort equivalent move.value??)

            // get randomMove (a random ushort)
            var prng = new System.Random();
            ushort randomMove = moves[prng.Next(0, moves.Length)];
            return new Move(randomMove);
        }

        public Move GetRandomBookMoveWeighted(ulong key) // smooth the weight (number player use) and random by weight ( cumulative me thod)
        {
            var p = bookPositions[key];
            ushort[] moves = new List<ushort>(p.numTimesMovePlayed.Keys).ToArray();
            int[] numTimesMovePlayed = new List<int>(p.numTimesMovePlayed.Values).ToArray();

            float[] moveWeights = new float[moves.Length];
            for (int i = 0; i < moveWeights.Length; i++)
            {
                moveWeights[i] = numTimesMovePlayed[i];
            }

            // Smooth weights to increase probability of rarer moves
            // (strength of 1 would make all moves equally likely)
            SmoothWeights(moveWeights, strength: 0.5f);

            float sum = 0;
            for (int i = 0; i < moveWeights.Length; i++)
            {
                sum += moveWeights[i];
            }

            float[] moveProbabilitiesCumul = new float[moveWeights.Length];
            float previousProbability = 0;
            for (int i = 0; i < moveWeights.Length; i++)
            {
                moveProbabilitiesCumul[i] = previousProbability + moveWeights[i] / sum;
                previousProbability = moveProbabilitiesCumul[i];
            }

            var prng = new System.Random();
            float t = (float)prng.NextDouble();


            for (int i = 0; i < moves.Length; i++)
            {
                if (t <= moveProbabilitiesCumul[i])
                {
                    return new Move(moves[i]);
                }
            }

            return new Move(moves[0]);
        }

        void SmoothWeights(float[] weights, float strength = 0.1f) // delta = weights[i] * factor (strength) => weights[i] += delta (Smooth)
        {
            float sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
            }
            float avg = sum / weights.Length;

            for (int i = 0; i < weights.Length; i++)
            {
                float offsetFromAvg = avg - weights[i];
                weights[i] += offsetFromAvg * strength;
            }
        }

        public void Add(ulong positionKey, Move move) // have a new Chess case with new move, add Chess Case with value = new BookPosition , add  new move to dictionary 
        {
            if (!bookPositions.ContainsKey(positionKey))
            {
                bookPositions.Add(positionKey, new BookPosition());
            }

            bookPositions[positionKey].AddMove(move);
        }

        public void Add(ulong positionKey, Move move, int numTimesPlayed) //similar with weight of move
        {
            if (!bookPositions.ContainsKey(positionKey))
            {
                bookPositions.Add(positionKey, new BookPosition());
            }

            bookPositions[positionKey].AddMove(move, numTimesPlayed);
        }
    }

    [System.Serializable]
    public class BookPosition
    {
        public Dictionary<ushort, int> numTimesMovePlayed; // number when player use for this 

        public BookPosition()
        {
            numTimesMovePlayed = new Dictionary<ushort, int>();
        }

        // Add Move  ( in that case move.value (ushort) represent for move) to Key of the dictionary  and value (  +1 if already exists , otherwise add new Key-value with default value=1))
        public void AddMove(Move move, int numTimesPlayed = 1)
        {
            ushort moveValue = move.Value;

            if (numTimesMovePlayed.ContainsKey(moveValue))
            {
                numTimesMovePlayed[moveValue]++;
            }
            else
            {
                numTimesMovePlayed.Add(moveValue, numTimesPlayed);
            }
        }
    }

}