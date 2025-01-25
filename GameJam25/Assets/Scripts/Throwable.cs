using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {
    }

    public IEnumerator ThrowTo(float x, float y, float seconds)
    {
        Debug.Log("Throwing " + gameObject.name);
        yield return Utilities.WaitForSeconds(seconds);
    }
}
