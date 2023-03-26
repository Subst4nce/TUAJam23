using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerFPS : MonoBehaviour
{


    [Range(0.1f, 5f)]  public float sensitivityX=1;
    [Range(0.1f, 5f)] public float sensitivityY=1;
    public PlayerInputHandlerPlatformer inputManager;

    private float sensMultiplier = 15f;

    private float desiredX;
    private float xRotation;

    bool isMoving;

    UEventHandler eventHandler=new UEventHandler();

    private void Awake()
    {
        isMoving = true;
    }

    private void Start()
    {

        CursorManager.instance.HideCursor();
        PauseMenu.OnPause.Subscribe(eventHandler,()=>isMoving=false);
        PauseMenu.OnResume.Subscribe(eventHandler,()=>isMoving=true);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void Update()
    {
        Look();
    }

  
    private void Look()
    {
        if (!isMoving) return;

        float mouseX = inputManager.input_look.value.x * sensitivityX * Time.smoothDeltaTime * sensMultiplier;
        float mouseY = inputManager.input_look.value.y * sensitivityY * Time.smoothDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        //orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
}
