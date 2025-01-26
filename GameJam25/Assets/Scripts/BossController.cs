using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{
    DialogBubbleController _dialogBubble;

    private void Start()
    {
        _dialogBubble = GetComponentInChildren<DialogBubbleController>();
    }

    public IEnumerator Say(string v)
    {
        Debug.Log("Boss says: \"" + v + "\"");
        return _dialogBubble.PopDialog(v);
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
