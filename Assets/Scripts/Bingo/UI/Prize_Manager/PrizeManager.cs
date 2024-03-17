using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class PrizeManager : MonoBehaviour
    {
        // test anim
        public Board board;

        public static PrizeManager instance;
        public Queue<PrizeWinner> prizeWiners;

        public TextMeshProUGUI nameOfPrize;
        public TextMeshProUGUI nameOfWinner;
        public Image avatarOfWinner;
        public Image avatarOfPrize;

        public bool onPrized = false;
        public BingoData data;


        // use for anim of prize panel
        public float displayPrizeInterval = 2f;
        public GameObject lighting;
        public GameObject[] stars;
        public Vector3[] starsPos;

        //Particle
        public ParticleSystem particle;
        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion
            prizeWiners = new Queue<PrizeWinner>();

            starsPos = new Vector3[stars.Length];
            for (int i = 0; i < stars.Length; i++)
            {
                starsPos[i] = stars[i].transform.position;
            }
        }

        private void Start()
        {
            CallerNumberController.instance.CallNumber += UpdateUIPrizeWinner;
            BingoGameController.instance.OnNewGameEvent += OnNewGame;
        }
        public void AddPrize(PrizeWinner _prizeWinner)
        {
            prizeWiners.Enqueue(_prizeWinner);
            onPrized = true;
        }

        public void UpdateUIPrizeWinner(int _callernumber)
        {

            StartCoroutine(DisplayAllPrizeWinner());
        }
        public IEnumerator DisplayAllPrizeWinner()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            if (onPrized)
            {



                while (prizeWiners.Count > 0)
                {
                    CallerNumberController.instance.TurnOffCalling();
                    PrizeWinner winner = prizeWiners.Dequeue();
                    DisplayPrizeWinner(winner);

                    yield return new WaitForSeconds(displayPrizeInterval);
                    CanvasManager.instance.TurnOffCanvas(CanvasType.GetPrize);
                    yield return new WaitForSeconds(0.1f);


                }

                CallerNumberController.instance.TurnOnCalling();
                onPrized = false;

                BingoGameController.instance.PlayerCantGetPrize();

                // check for BINGO prize and END game
                if (BingoGameController.instance.havingPrizes[5] == false)
                {
                    BingoGameController.instance.OnEndGame();
                }
            }

        }
        public void DisplayPrizeWinner(PrizeWinner _prizeWinner)
        {
            NewSetUpPrizeWinner(_prizeWinner);
            CanvasManager.instance.TurnOnCanvas(CanvasType.GetPrize);
            SoundManager.instance.PlaySFXSound(SFXSound.Correct);
            if (_prizeWinner.prize == Prize.BINGO && _prizeWinner.winner.owner == BoardOwner.Player)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }

            StartCoroutine(PrizePanelAnim());

        }

        public void NewSetUpPrizeWinner(PrizeWinner _prizeWinner)
        {
            nameOfPrize.text = _prizeWinner.prize.ToString();

            if (_prizeWinner.winner.owner == BoardOwner.Player)
            {
                nameOfWinner.text = "YOU";
            }
            else
            {
                nameOfWinner.text = "Player" + "  " + UnityEngine.Random.Range(1, 5);
            }

            avatarOfPrize.sprite = data.prizesConditionSprite[(int)_prizeWinner.prize];
            avatarOfWinner.sprite = data.avatarsOfWinner[(int)_prizeWinner.winner.owner];

        }

        public IEnumerator PrizePanelAnim()
        {
            #region StartAnim
            //lighting
            lighting.transform.localScale = Vector3.zero;
            lighting.transform.localRotation = Quaternion.identity;

            //Star
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].transform.position = avatarOfWinner.transform.position;
            }

            //prize
            avatarOfPrize.transform.localScale = Vector3.zero;

            #endregion

            #region Anim

            //lighting
            LeanTween.scale(lighting, Vector3.one, displayPrizeInterval / 4).
                setOnComplete(() => { LeanTween.rotateAround(lighting, Vector3.forward, -60, displayPrizeInterval / 4); });

            //star
            for (int i = 0; i < stars.Length; i++)
            {
                LeanTween.move(stars[i], starsPos[i], displayPrizeInterval / 4).setEase(LeanTweenType.easeOutBack);
            }

            //prize
            LeanTween.scale(avatarOfPrize.gameObject, Vector3.one, displayPrizeInterval / 4);
            #endregion


            yield return new WaitForSeconds(2f);
            CanvasManager.instance.TurnOffCanvas(CanvasType.GetPrize);
        }

        public void TestAnim()
        {
            var test = new PrizeWinner(board, Prize.Column);
            DisplayPrizeWinner(test);


        }
        public void OnNewGame()
        {
            prizeWiners.Clear();
        }
    }
}

