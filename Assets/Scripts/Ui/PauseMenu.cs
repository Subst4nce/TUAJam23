using BrunoMikoski.AnimationSequencer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class PauseMenu : MonoBehaviour
{
    public PlayerInputHandlerPlatformer inputhandler;

    public AnimationSequencerController animator;

    public static UEvent OnPause = new UEvent();
    public static UEvent OnResume = new UEvent();

    UEventHandler eventHandler = new UEventHandler();

    bool isPaused;

    void Start()
    {
        inputhandler.input_pause.Onpressed.Subscribe(eventHandler, PauseResume);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }
    public void PauseResume()
    {
        if (isPaused)
            Resume();
        else
            Pause();

        isPaused = !isPaused;
    }

    public void Resume()
    {
        OnResume.TryInvoke();
        animator.PlayBackwards();

    }
    public void Pause()
    {
        OnPause.TryInvoke();
        animator.PlayForward(false);
    }


    public void QuitGame()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
