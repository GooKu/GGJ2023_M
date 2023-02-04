using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioPlayer Instance;

        [SerializeField] AudioSource _music;
        [SerializeField] AudioSource _effect;

        [SerializeField] AudioClip[] _audioClips;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void PlayEffect(int clipID)
        {
            _effect.PlayOneShot(_audioClips[clipID]);
        }

        public void PlayMusic(int clipID)
        {
            _music.clip = _audioClips[clipID];
            _music.Play();
        }
    }
}
