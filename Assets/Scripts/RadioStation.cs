using NaughtyAttributes;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static SoundUtils;


[Serializable]
public struct RadioChannel
{
    public string name;
    public Sound[] tracks;


}

public class RadioStation : MonoBehaviour
{

    public static RadioStation instance;
    public AudioSource source;
    public RadioChannel[] channels;

    public AudioSource noiseTrack;
    public float noiseVolume;

    RadioChannel? channelPlaying;

    int radioIndex = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayChannel(channels[0]);
       
    }


    [Button("NextChannel")]
    public void PlayNextChannel()
    {
        radioIndex = radioIndex >= channels.Length - 1 ? 0 : radioIndex + 1;

        PlayChannel(channels[radioIndex]);
    }

    [Button("PrevChannel")]
    public void PlayPreviousChannel()
    {
        radioIndex = radioIndex <= 0 ? channels.Length - 1 : radioIndex - 1;

        PlayChannel(channels[radioIndex]);
    }



    public async void PlayChannel(RadioChannel radioChannel)
    {
        //float initNoiseVolume= noiseTrack.volume;
        source.Stop();
        channelPlaying = radioChannel;


        noiseTrack.volume = noiseVolume;
        await Task.Delay(650);

        noiseTrack.volume = 0;
        source.PlayRandomSound(radioChannel.tracks, false, true);


    }

}
