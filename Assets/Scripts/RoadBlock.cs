using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock : MonoBehaviour
{
    public TriggerChecker endingZoneChecker;
    public UEventHandler eventHandler = new UEventHandler();

    public Transform startConnector;
    public Transform endConnector;
    bool hasCrossed = false;

    void Start()
    {
        endingZoneChecker.OnTriggered.Subscribe(eventHandler, Crossed);
    }

    private void Crossed(Transform t)
    {
        if (hasCrossed) return;

        hasCrossed = true;
        RoadBlockGenerator.Instance.BlockCrossed(this);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
