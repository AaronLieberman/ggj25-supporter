using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingGear : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {
    }

    public void SetBubblesEnabled(bool v)
    {
        Debug.Log(v ? "DivingGear bubbles enabled" : "DivingGear bubbles disabled");
    }
}
