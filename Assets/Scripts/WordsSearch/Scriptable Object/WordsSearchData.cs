using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
    [CreateAssetMenu(menuName = "Scriptable Object/Words Search/Data")]
    public class WordsSearchData : ScriptableObject
    {

        [SerializeField] private GameObject cellObject;
        public GameObject CellObject => cellObject;

        [SerializeField] private Color[] lineColors = new Color[8];
        public Color[] LineColors => lineColors;

        [SerializeField] private GameObject line;
        public GameObject Line => line;

        [SerializeField] private Sprite strikeThroughSprite;
        public Sprite StrikeThroughSprite => strikeThroughSprite;

    }
}

