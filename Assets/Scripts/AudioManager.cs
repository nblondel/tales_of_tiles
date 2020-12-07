using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public AudioSource musicAudioSource;
    public AudioSource ambientAudioSource;
    public AudioMixer audioMixer;

    public AudioClip musicAudioClip;
    [OptionalField] public AudioClip ambientAudioClip;

    // Start is called before the first frame update
    private void Start() {
        musicAudioSource.clip = musicAudioClip;
        musicAudioSource.Play();

        if (ambientAudioClip != null) {
            ambientAudioSource.clip = ambientAudioClip;
            ambientAudioSource.Play();
        }
    }

    public void SetSoundVolume(float volume) {
        if (volume <= -40f) {
            audioMixer.SetFloat("SoundsVolume", -80f);
        } else {
            audioMixer.SetFloat("SoundsVolume", volume);
        }
    }

    public void SetMusicVolume(float volume) {
        if (volume <= -40f) {
            audioMixer.SetFloat("MusicVolume", -80f);
        } else {
            audioMixer.SetFloat("MusicVolume", volume);
        }
    }
}