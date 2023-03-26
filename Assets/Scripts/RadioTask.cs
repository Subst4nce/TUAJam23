using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTask : MonoBehaviour, PlayerTask
{
    public DialogueSentence[] initialText;

    public bool isRunning;
    public bool isIntroDone;

    public string[] hintsFailed;

    public string againFailed;
    public string fastFailed;
    public string success;

    [ReadOnly] public string correctChannel;

    [ReadOnly]
    public List<string> attempts;

    public float listenTime;

    int attemptsMade = 0;

    Coroutine listenTask;

    UEventHandler eventHandler = new UEventHandler();
    private void Start()
    {
        attempts = new List<string>();
        RadioStation.instance.OnRadioChanged.Subscribe(eventHandler, RadioChanged);
        var channels = RadioStation.instance.channels;
        correctChannel = channels[Random.Range(1, channels.Length)].name;
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    [Button("Begin Task")]
    public void BeginTask()
    {
        isRunning = true;
        DialogueWriter.instance.ReadSentences(initialText);
        DialogueWriter.instance.OnDialogueEnded.Subscribe(eventHandler, DialogueEnded);

    }

    void DialogueEnded()
    {
        if (!isRunning) return;
        if (!isIntroDone)
        {
            isIntroDone = true;
        }
        else
        {

        }


    }

    void RadioChanged(string radioChannel)
    {
        if (!isRunning) return;

        if (!isIntroDone) return;

        if (listenTask != null)
        {
            StopCoroutine(listenTask);
            listenTask = null;
            DialogueWriter.instance.ReadSingleSentence(success);
        }

        listenTask = StartCoroutine(ListenToMusic(radioChannel));

    }

    IEnumerator ListenToMusic(string radioChannel)
    {
        yield return new WaitForSeconds(listenTime);

        if (attempts.Contains(radioChannel))
        {
            DialogueWriter.instance.ReadSingleSentence(againFailed);
            yield return new WaitForEndOfFrame();
        }

        attempts.Add(radioChannel);
        if (radioChannel == correctChannel)
        {
            isRunning = false;
            DialogueWriter.instance.ReadSingleSentence(success);
        }
        else
        {
            DialogueWriter.instance.ReadSingleSentence(hintsFailed[attemptsMade]);
            attemptsMade++;
        }

        listenTask = null;
    }



}