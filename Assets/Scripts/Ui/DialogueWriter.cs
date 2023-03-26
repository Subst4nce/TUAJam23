using BrunoMikoski.AnimationSequencer;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueWriter : MonoBehaviour
{
    public static DialogueWriter instance;

    public PlayerInputHandlerPlatformer inputHandlerPlatformer;
    public TMP_Animated sourceText;
    public AnimationSequencerController animator;
    public AnimationSequencerController showIconAnimator;
    public AnimationSequencerController hideIconAnimator;

    [Multiline]
    public string[] testSentences;

    List<string> sentencesToRead;
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

            ReadNextSentence();
        }
    }

    [Button("Read Test")]
    public void ReadTest()
    {
        ReadSentences(testSentences);
    }
    public bool HasMoreSentecesToRead() => sentencesToRead.Count > 0;


    public void ReadSentences(string[] sentences)
    {

        if (sentencesToRead == null)
        {
            sentencesToRead = new List<string>();
        }

        sentencesToRead.AddRange(sentences);
        ReadNextSentence();
    }

    public void ReadNextSentence()
    {
        isSentenceFinished = false;

        if (!HasMoreSentecesToRead())
        {
            HideDialogue();
            return;
        }

        ReadSentence(sentencesToRead[0]);
    }


    public void ReadSentence(string sentence)
    {
        ShowDialogue();

        showIconAnimator.Pause();
        hideIconAnimator.Play();

        waitingToContinue = false;
        isSentenceFinished = false;

        sourceText.ReadText(sentence);
    }

    void PressedNext()
    {

    }

    void SentenceFinished()
    {
        isSentenceFinished = true;
        sentencesToRead.RemoveAt(0);

        if (HasMoreSentecesToRead())
        {
            waitingToContinue = true;

            showIconAnimator.Play();

            hideIconAnimator.Pause();
        }
        else
        {
            HideDialogue();
        }
    }


    void ShowDialogue()
    {
        if (dialogueShown) return;

        animator.Play();
        dialogueShown = true;
    }

    void HideDialogue()
    {
        if (!dialogueShown) return;

        animator.PlayBackwards();

        dialogueShown = false;

    }

}
