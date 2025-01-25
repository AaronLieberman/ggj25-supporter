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
}
