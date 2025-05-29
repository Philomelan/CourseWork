using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager main;

    private AudioSource audioSource;
    
    private void Start()
    {
        main = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    public void PlaySFX(AudioClip clip) {
        if (clip == null) {
            return;
        }

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clip);
    }
}
