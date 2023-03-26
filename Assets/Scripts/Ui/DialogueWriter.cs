using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UEventHandler;

[Serializable]
public struct DialogueSentence
{
    public float startDelay;
    public string text;
}

public class DialogueWriter : MonoBehaviour
{
    public static DialogueWriter instance;

    public PlayerInputHandlerPlatformer inputHandlerPlatformer;
    public TMP_Animated sourceText;

    public AnimationSequencerController showIconAnimator;
    public AnimationSequencerController hideIconAnimator;

    public CanvasGroup canvasGroup;

    //public float dialogueAnimOffset;
    public float dialogueAnimDuration;
    public Ease dialogeAnimEaseIn;
    public Ease dialogeAnimEaseOut;

    public UEvent OnDialogueEnded = new UEvent();

    public DialogueSentence[] testSentences;

    List<DialogueSentence> sentencesToRead;
    bool waitingToContinue, isSentenceFinished, dialogueShown;

    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sourceText.OnDialogueFinished.Subscribe(eventHandler, SentenceFinished);
    }


    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayingNext();
    }

    void CheckForPlayingNext()
    {
        if (Input.anyKeyDown && isSentenceFinished && waitingToContinue)
        {
            if (inputHandlerPlatformer.input_pause.value > 0) return;

            if (HasMoreSentecesToRead())
            {
                ReadNextSentence();
            }
            else
            {
                HideDialogue();
                OnDialogueEnded.TryInvoke();
            }

        }
    }

    [Button("Read Test")]
    public void ReadTest()
    {
        ReadSentences(testSentences);
    }


    public bool HasMoreSentecesToRead() => sentencesToRead.Count > 0;

    public void ReadSingleSentence(string sentence)
    {
        ReadSentences(new DialogueSentence[] { new DialogueSentence { text = sentence, startDelay = 0 } });
    }

    public void ReadSentences(DialogueSentence[] sentences)
    {
        if (sentencesToRead == null)
        {
            sentencesToRead = new List<DialogueSentence>();
        }

        sentencesToRead.AddRange(sentences);
        ReadNextSentence();

    }

    //public void ReadSentences(string[] sentences)
    //{

    //    //if (sentencesToRead == null)
    //    //{
    //    //    sentencesToRead = new List<string>();
    //    //}

    //    //sentencesToRead.AddRange(sentences);
    //    //ReadNextSentence();
    //}

    private void ReadNextSentence()
    {
        isSentenceFinished = false;

        if (!HasMoreSentecesToRead())
        {
            HideDialogue();
            return;
        }

        StartCoroutine(ReadSentence(sentencesToRead[0]));
    }


    IEnumerator ReadSentence(DialogueSentence sentence)
    {
        if (sentence.startDelay > 0)
        {
            HideDialogue();
            yield return new WaitForSeconds(sentence.startDelay);
        }

        ShowDialogue();



        waitingToContinue = false;
        isSentenceFinished = false;

        sourceText.ReadText(sentence.text);
    }


    void SentenceFinished()
    {
        isSentenceFinished = true;
        sentencesToRead.RemoveAt(0);

        waitingToContinue = true;

        showIconAnimator.Play();

        hideIconAnimator.Pause();

    }


    void ShowDialogue()
    {
        if (dialogueShown) return;

        //transform.DOMoveY(0, dialogueAnimDuration).SetEase(dialogeAnimEaseIn);
        canvasGroup.DOFade(1, dialogueAnimDuration).SetEase(dialogeAnimEaseIn);

        dialogueShown = true;
    }

    void HideDialogue()
    {
        if (!dialogueShown) return;

        canvasGroup.DOFade(0, dialogueAnimDuration).SetEase(dialogeAnimEaseOut);
        //transform.DOMoveY(dialogueAnimOffset, dialogueAnimDuration).SetEase(dialogeAnimEaseOut);

        dialogueShown = false;

    }

}
