using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource source;

    private void Start()
    {
        //Set previous volume
        if (PlayerPrefs.HasKey("Volume"))
            source.volume = PlayerPrefs.GetFloat("Volume");
        else
        {
            PlayerPrefs.SetFloat("Volume", 1);
            source.volume = PlayerPrefs.GetFloat("Volume");
        }
    }

    public void StartMusic()
    {
        source.Play();
    }

    public void ChangeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Volume", newVolume);
        source.volume = newVolume;
    }
}
