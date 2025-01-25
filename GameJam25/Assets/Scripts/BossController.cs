using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {
    }

    public IEnumerator Say(string v, float seconds)
    {
        Debug.Log("Boss says: \"" + v + "\"");
        yield return Utilities.WaitForSeconds(seconds);
    }
    public void StartShooters()
    {
        Debug.Log("Activating all children of boss");

        //TODO: This turns on everything, not just shooters
        gameObject.SetActiveRecursively(true);
    }
}
