using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Chess
{
    public class EndGameUIController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gameResult;
        [SerializeField] TextMeshProUGUI playerResult;

        [SerializeField] string winText = "YOU WIN!!";
        [SerializeField] string loseText = "YOU LOSE!!";
        [SerializeField] string drawText = "DRAW!!";
        [SerializeField] string checkmateText = "Check Mate";
        [SerializeField] string threeFoldText = "Threefold Repetition Rule";
        [SerializeField] string fiftyMoveText = "Fifty-move Rule";
        [SerializeField] string staleMateText = "Stalemate";
        [SerializeField] string insufficientMatText = "Insufficient Material";

        [SerializeField] ParticleSystem winParticle;

        private void Awake()
        {
            if (winParticle.isPlaying)
                winParticle.Stop();

            GameManager.OnEndgameEvent += OnEndgame;
            winText = "YOU WIN!!";
            loseText = "YOU LOSE!!";
            drawText = "DRAW!!";
            checkmateText = "Check Mate";
            threeFoldText = "Threefold Repetition Rule";
            fiftyMoveText = "Fifty-move Rule";
            staleMateText = "Stalemate";
            insufficientMatText = "Insufficient Material";
        }

        

        void OnEndgame(Result gameResult)
        {
            switch (gameResult)
            {
                case Result.WhiteIsMated:
                    LoseEffect();
                    SetUpEndGameText(checkmateText, loseText);
                    break;
                case Result.BlackIsMated:
                    WinEffect();
                    SetUpEndGameText(checkmateText, winText);
                    break;
                case Result.FiftyMoveRule:
                    WinEffect();
                    SetUpEndGameText(fiftyMoveText, drawText);
                    break;
                case Result.InsufficientMaterial:
                    WinEffect();
                    SetUpEndGameText(insufficientMatText, drawText);
                    break;
                case Result.Repetition:
                    WinEffect();
                    SetUpEndGameText(threeFoldText, drawText);
                    break;
                case Result.Stalemate:
                    WinEffect();
                    SetUpEndGameText(staleMateText, drawText);
                    break;

            }
        }

        void SetUpEndGameText(string _gameResult, string _playerResult)
        {
            gameResult.text = _gameResult;
            playerResult.text = _playerResult;
        }

        public void LoseEffect()
        {
            SoundManager.instance.PlaySFXSound(SFXSound.GameOver);
        }

        public void WinEffect()
        {
            winParticle.gameObject.SetActive(true);
            winParticle.Play();
            SoundManager.instance.PlaySFXSound(SFXSound.Win);
        }


    }
}

