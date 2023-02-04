using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ23M
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource _music;
        [SerializeField] AudioSource _effect;

        [SerializeField] AudioClip[] _audioClips;

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
