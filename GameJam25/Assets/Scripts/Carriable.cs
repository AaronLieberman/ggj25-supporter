using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Carriable : MonoBehaviour
{
    [SerializeField] private List<string> interactionTags;
    [SerializeField] private float dropWait = 1.0f;
    private float lastDropTime;
    [HideInInspector] public bool BeingCarried;

    private void Update()
    {
        if (BeingCarried)
        {
            if (Input.GetButtonDown("Dash"))
            {
                Drop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time < lastDropTime + dropWait) { return; }

        if (interactionTags.Contains(collision.tag))
        {
            Carry(collision.transform);
        }
    }

    public void Carry(Transform carrierTransform)
    {
        if (BeingCarried) { return; }

        transform.SetParent(carrierTransform);
        transform.localPosition = new Vector3(1.0f, 0.0f, 0.0f);
        BeingCarried = true;
    }

    public void Drop()
    {
        if (!BeingCarried) { return; }

        lastDropTime = Time.time;

        transform.SetParent(null);
        BeingCarried = false;
    }
}
