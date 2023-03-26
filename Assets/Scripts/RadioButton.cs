using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButton :  MonoBehaviour, Interactable
{
    public bool isNextBttn;

    public List<Renderer> renderers;
    public List<Renderer> GetInteractableMeshes() => renderers;

    public void Interact()
    {
        if (isNextBttn)
            RadioStation.instance.PlayNextChannel();
        else
            RadioStation.instance.PlayPreviousChannel();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
