using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameAudio : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>();
    private static List<AudioClip> instanceAudioClips = new List<AudioClip>();
    private static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        instanceAudioClips = audioClips;
    }

    //: Base Single Audio Instance
    static void PlayOneInstanceAudio(AudioClip audioClip)
    {
        try
        {
            audioSource.PlayOneShot(audioClip);
        }
        catch (Exception e)
        {
            Console.WriteLine("NULL AUDIOCLIP " + e);
            throw;
        }
    }

    public static void PlaySwimmerDamaged()
    {
        PlayOneInstanceAudio(instanceAudioClips[0]);
    }

    public static void PlayAnchorSound()
    {
        PlayOneInstanceAudio(instanceAudioClips[1]);
    }

    public static void PlayMineSound()
    {
        PlayOneInstanceAudio(instanceAudioClips[2]);
    }

    public static void PlayFishSound()
    {
        PlayOneInstanceAudio(instanceAudioClips[3]);
    }

    public static void PlaySharkDamaged()
    {
        PlayOneInstanceAudio(instanceAudioClips[4]);
    }

    public static void PlaySharkPreAttack()
    {
        PlayOneInstanceAudio(instanceAudioClips[5]);
    }

    public static void PlaySharkAttack()
    {
        PlayOneInstanceAudio(instanceAudioClips[6]);
    }

    public static void PlaySharkShortPostAttack()
    {
        PlayOneInstanceAudio(instanceAudioClips[7]);
    }

    public static void PlaySharkLostPostAttack()
    {
        PlayOneInstanceAudio(instanceAudioClips[8]);
    }
}
