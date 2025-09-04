using UnityEngine;
using System.Collections.Generic;
using System;

namespace TopDown.SceneManagment
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("General Ambient Sound")]
        [SerializeField] private AudioClip ambientAudioSource;
        [SerializeField][Range(0f, 1f)] private float ambientVolume = 0.5f;
        [Header("Player SFXs")]
        [SerializeField] private AudioClip swordAttack;
        [SerializeField] private AudioClip bowAttack;
        [SerializeField] private AudioClip staffAttack;
        [SerializeField][Range(0f, 1f)] private float playerAttackVolume = 0.4f;
        [SerializeField] private AudioClip playerHurt;
        [SerializeField][Range(0f, 1f)] private float playerHurtVolume = 0.4f;


        void Start()
        {
            PlayAmbientSound(ambientVolume);
        }

        public void PlayAmbientSound(float ambientVolume)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = ambientAudioSource;
            audioSource.volume = ambientVolume;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void PlayPlayerAttackSFX(string weaponType)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            switch (weaponType)
            {
                case "Sword":
                    audioSource.clip = swordAttack;
                    break;
                case "Bow":
                    audioSource.clip = bowAttack;
                    break;
                case "Staff":
                    audioSource.clip = staffAttack;
                    break;
                default:
                    Debug.LogWarning("Unknown weapon type: " + weaponType);
                    return;
            }
            audioSource.volume = playerAttackVolume;
            audioSource.Play();
            // Destroy(audioSource, audioSource.clip.length);
        }

        public void PlayPlayerHurtSFX()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = playerHurt;
            audioSource.volume = playerHurtVolume;
            audioSource.Play();
        }
    }
}
