using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    [CreateAssetMenu(fileName = "BingoData", menuName = "Scriptable Object / BINGO Data")]
    public class BingoData : ScriptableObject
    {
        // prize condition and check get and don't get prize
        public Sprite[] prizesConditionSprite;
        public Sprite[] prizesConditionSpriteOff;
        public Sprite getSprite;
        public Sprite dontGetPrize;

        // B,I,N,G,O header
        public Sprite[] columnsHeaderSprite;

        //avatar of Player , Boss 1 , Boss 2 , Boss 3 
        public Sprite[] avatarsOfWinner;

        // star in center cell
        public Sprite centerStar;

        //Balls sprite
        public Sprite[] ballSprites;
    }

}
