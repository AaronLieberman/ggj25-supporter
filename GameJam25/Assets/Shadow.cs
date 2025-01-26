using System;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    void LateUpdate()
    {
        transform.Translate(0, 0, -transform.position.z);
        var opacity = 1 - Math.Min(1, Vector3.Distance(transform.position, transform.parent.position) / 40);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, opacity);
    }
}
