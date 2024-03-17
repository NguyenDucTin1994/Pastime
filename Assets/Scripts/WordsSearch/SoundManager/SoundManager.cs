using Bingo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace WordsSearch
{
    public enum SFXSound
    {
        Click,
        Correct,
        Win,
    }
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [SerializeField] AudioMixer wordSearchMixer;

        [SerializeField] AudioMixerGroup wordSearchGroup;
        [SerializeField] AudioMixerGroup sfxGroup;

        [SerializeField] AudioSource sfxSource;

        public AudioClip[] sfxSounds;


        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion

            sfxSource.outputAudioMixerGroup = sfxGroup;
        }

        public void PlaySFXSound(SFXSound _sfxSound)
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }
            sfxSource.PlayOneShot(sfxSounds[(int)_sfxSound]); 
            //Debug.Log(_sfxSound.ToString());
        }

    }
}