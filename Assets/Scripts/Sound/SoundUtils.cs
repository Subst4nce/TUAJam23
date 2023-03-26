using System;
using UnityEngine;
//using Random = System.Random;

public static class SoundUtils
{

    [Serializable]
    public struct Sound
    {
        [Range(0, 3)] public float volume;
        public AudioClip clip;
    }

    public static void PlaySound(this AudioSource source, Sound sound, bool oneShot = true, bool randomTime = false)
    {
        if (oneShot)
            source.PlayOneShot(sound.clip, sound.volume);
        else
        {
            source.clip = sound.clip;
            source.volume = sound.volume;

            source.Play();

            if (randomTime)
                source.timeSamples = UnityEngine.Random.Range(0, sound.clip.samples);
        }
    }

    public static void PlaySoundVol(this AudioSource source, Sound sound, float volMultiplier, bool oneShot = true, bool randomTime = false)
    {
        source.PlayOneShot(sound.clip, sound.volume * volMultiplier);
    }

    public static void PlayRandomSound(this AudioSource source, Sound[] sounds, bool oneShot = true, bool randomTime = false)
    {
        PlaySound(source, sounds[UnityEngine.Random.Range(0, sounds.Length)], oneShot, randomTime);
    }
}
