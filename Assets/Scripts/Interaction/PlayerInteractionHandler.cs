using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class PlayerInteractionHandler : MonoBehaviour
{
    public static UEvent<Interactable, Transform> OnInteractableAppeared = new UEvent<Interactable, Transform>();
    public static UEvent OnInteractableDisappeared = new UEvent();
    public static UEvent<Vector3> OnSplash = new UEvent<Vector3>();
    static GameObject objectToInteract = null;

    //public PlayerAimController aimController;
    public PlayerInputHandlerPlatformer inputManager;
    public Camera mainCamera;
    public float castWidth = .5f;
    public float castLength = 5;
    public LayerMask castMask;

    public bool isInteracting;

    private UEventHandler eventHandler = new UEventHandler();

    private void Start()
    {
        inputManager.input_interact.Onpressed.Subscribe(eventHandler, TryInteract);
        Grabbable.OnGrabbed.Subscribe(eventHandler, (x) => isInteracting = true);
        Grabbable.OnReleased.Subscribe(eventHandler, () => isInteracting = false);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }
    //public static bool IsInteractableNearby() => objectToInteract != null;

    private RaycastHit hit;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hit.point, castWidth);

    }

    private void Update()
    {
        if (isInteracting)
        {
            objectToInteract = null;
            return;
        }

        Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * castLength, Color.red);

        var hasHit = Physics.SphereCast(mainCamera.transform.position, castWidth, mainCamera.transform.forward, out hit, castLength, castMask);


        if (!hasHit || (hasHit && hit.transform.tag != "Interactable"))
        {
            if (objectToInteract != null)
                OnInteractableDisappeared.TryInvoke();

            objectToInteract = null;
            return;
        }
        //if (hit.transform.parent == null) return;

        if (hit.transform.tag == "Interactable" && hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (objectToInteract == null || objectToInteract.transform.GetInstanceID() != hit.transform.GetInstanceID())
            {
                if (objectToInteract != null)                           //It was a dif object so must first make disappear the last one 
                    OnInteractableDisappeared.TryInvoke();

                //if ()
                {
                    objectToInteract = hit.transform.gameObject;
                    OnInteractableAppeared.TryInvoke(interactable, hit.transform);
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.parent == null) return;


    //    if (other.transform.parent.tag == "Interactable")
    //    {
    //        if (objectToInteract == null || objectToInteract.transform.position != other.transform.position)
    //        {
    //            objectToInteract = other.gameObject;
    //            if (objectToInteract.transform.parent.TryGetComponent<Interactable>(out Interactable interactable))
    //                OnInteractableAppeared.TryInvoke(interactable,other.transform, interactable.GetOffset());
    //        }
    //    }
    //    else if (other.transform.parent.tag == "KillBound")
    //    {
    //        LevelManager.currentManager.ResetPlayer();
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.parent == null) return;

    //    if (other.transform.parent.tag == "Interactable")
    //    {
    //        if (objectToInteract != null && objectToInteract.transform.position == other.transform.position)
    //        {
    //            objectToInteract = null;
    //            OnInteractableDisappeared.TryInvoke();
    //        }
    //    }
    //}

    public void TryInteract()
    {
        if (objectToInteract == null || objectToInteract.transform.tag != "Interactable") return;
        //if (PlayerCutsceneManager.isInCutscene) return;

        objectToInteract.transform.GetComponent<Interactable>().Interact();
        OnInteractableDisappeared.TryInvoke();
    }

}
