using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = AppManager.Instance.GetComponent<SoundManager>();
            }

            return instance;
        }
    }

    public enum SoundEffect
    {
        Undefined,
        DeepOneWalk,
        TurnChange,
    }

    [SerializeField]
    private AudioClip deepOneWalk;
    [SerializeField]
    private AudioClip turnChange;

    private AudioSource audioPlayer;

    private AudioClip GetClip(SoundEffect effect)
    {
        switch (effect)
        {
            case SoundEffect.DeepOneWalk:
                return deepOneWalk;
            case SoundEffect.TurnChange:
                return turnChange;
        }

        if (effect != SoundEffect.Undefined)
        {
            Debug.LogError("No clip for " + effect);
        }
        
        return null;
    }

    void Start()
    {
        audioPlayer = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(SoundEffect sound)
    {
        var clip = GetClip(sound);
        if (clip && audioPlayer)
        {
            audioPlayer.PlayOneShot(clip);
        }

    }
}
