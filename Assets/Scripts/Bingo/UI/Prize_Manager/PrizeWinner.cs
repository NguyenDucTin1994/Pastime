using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    [Serializable]
    public class PrizeWinner
    {
        public Board winner;
        public Prize prize;
        public PrizeWinner(Board _winner, Prize _prize)
        {
            winner = _winner;
            prize = _prize;
        }
    }
}


