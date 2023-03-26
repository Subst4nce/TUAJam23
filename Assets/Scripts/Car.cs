using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed = 1;
    //public float amplitude; // the amount it moves
    //public float frequency;
    //private float initialYPos;
    void Start()
    {
        //initialYPos = transform.position.y;
    }

    void FixedUpdate()
    {

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //transform.position = new Vector3(transform.position.x, initialYPos + Mathf.Sin(frequency * Time.deltaTime) * amplitude, transform.position.z);
    }
}
