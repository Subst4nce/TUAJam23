using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int maxKarmaPoints;


    public int currentKarmaPoints;


    void Start()
    {

    }

    void Update()
    {

    }

    public void AddKarma(int pointsToAdd)
    {
        currentKarmaPoints += pointsToAdd;

        if (currentKarmaPoints >= maxKarmaPoints)
        {
            PreDeath();
        }

    }

    public void PreDeath()
    {

    }

}
