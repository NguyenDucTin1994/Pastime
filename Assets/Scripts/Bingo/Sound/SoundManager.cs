using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace Bingo
{
    public enum SFXSound
    {
        Correct,
        Incorrect,
        Move,
        Bingo
    }
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [SerializeField] AudioMixer bingoMixer;
        [SerializeField] AudioMixer sfxMixer;
        [SerializeField] AudioMixerGroup bingoGroup;
        [SerializeField] AudioMixerGroup sfxGroup;
        [SerializeField] AudioSource speakNumsSource;
        [SerializeField] AudioSource sfxSource;

        public AudioClip[] sfxSounds;

        public AudioClip[] numbersClip;

        public string fileName;
        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion

            fileName = "Bingo";
            numbersClip = new AudioClip[75];
            speakNumsSource.outputAudioMixerGroup = bingoGroup;
            sfxSource.outputAudioMixerGroup = sfxGroup;
        }

        private void Start()
        {
            CallerNumberController.instance.CallNumber += PlayCallingNumber;
            LoadNumbersFile(fileName);
        }
        void LoadNumbersFile(string _fileName)
        {
            numbersClip = Resources.LoadAll<AudioClip>(_fileName);
        }

        public void PlayCallingNumber(int _newNumber)
        {
            StartCoroutine(SpeakNumber(_newNumber));
        }

        public IEnumerator SpeakNumber(int _number)
        {
            yield return new WaitForSeconds(0.3f);
            speakNumsSource.PlayOneShot(numbersClip[_number - 1]);
        }

        public void PlaySFXSound(SFXSound _sfxSound)
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }
            sfxSource.PlayOneShot(sfxSounds[(int)_sfxSound]);
        }
    }
}

