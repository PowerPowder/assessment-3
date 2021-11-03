using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public enum Music { Normal, Scared, Dead, Menu }
    public Music currentTrack = Music.Normal;
    private Music prevTrack;

    [SerializeField]
    private AudioClip normal;
    [SerializeField]
    private AudioClip scared;
    [SerializeField]
    private AudioClip dead;
    [SerializeField]
    private AudioClip menu;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = normal;
        audioSource.Play();
    }

    void Update()
    {
        if (prevTrack != currentTrack)
        {
            switch (currentTrack)
            {
                case Music.Normal:
                    audioSource.clip = normal;
                    break;
                case Music.Scared:
                    audioSource.clip = scared;
                    break;
                case Music.Dead:
                    audioSource.clip = dead;
                    break;
                case Music.Menu:
                    audioSource.clip = menu;
                    break;
            }

            audioSource.Play();
        }

        prevTrack = currentTrack;
    }
}
