using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
    public class EndGameUI : MonoBehaviour
    {
        [SerializeField] ParticleSystem winParticle;

        private void Awake()
        {
            if (winParticle.isPlaying)
                winParticle.Stop();
        }

        private void Start()
        {
            GameManager.EndGameEvent += EndGame;
        }

        public void EndGame()
        {
            winParticle.gameObject.SetActive(true);
            winParticle.Play();
        }
    }
}

