using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        foreach (var pl in GetComponentsInChildren<ProjectileLauncher>(true))
        {
            pl.gameObject.SetActive(true);
        }
    }
}
