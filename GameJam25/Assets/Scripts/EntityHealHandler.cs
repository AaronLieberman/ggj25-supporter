using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealHandler : MonoBehaviour
{
    [SerializeField]
    private List<string> interactionTags;
    [SerializeField]
    private int healAmount = 1;
    EntityResources entityResources;

    public bool InHurtState { get; private set; }

    void AttachPlayerResources()
    {
        entityResources = transform.parent.parent.GetComponent<EntityResources>();
    }

   
}
