using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class Player
    {
        public event System.Action<Move> onDoMove;

        public abstract void Update();

        public abstract void FinishThisTurn();

        public virtual void DoMove(Move move)
        {
            onDoMove?.Invoke(move);
        }

        public virtual void ChoosePromotionMove(int value)
        {

        }

        public virtual void DoPromotionMove()
        {

        }

        public void MakeMoveSound(bool _isAIPlayer)
        {
            if(_isAIPlayer)
            {
                SoundManager.instance.PlaySFXSound(SFXSound.Move);
            }
               
        }
            
    }
}