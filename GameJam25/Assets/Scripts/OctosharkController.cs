using System.Collections.Generic;
using UnityEngine;

public class OctosharkController : MonoBehaviour
{
    [HideInInspector] public EntityResources EntityResources;

    enum OctosharkState { Idle, Running }

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;

    [SerializeField] float PursueInterval = 0.1f;
    Transform _pursueTarget;

    ControlledMover _controlledMover;

    private void Awake()
    {
        EntityResources = GetComponent<EntityResources>();
    }

    void Start()
    {
        _controlledMover = GetComponent<ControlledMover>();

        if (EntityResources) EntityResources.Death += Die;

        AttachPlayerTarget();
    }

    void AttachPlayerTarget()
    {
        _pursueTarget = Utilities.GetRootComponent<PlayerController>().transform;
    }

    void Update()
    {
        var damageHandler = GetComponentInChildren<EntityDamageHandler>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        bool isInHurtState = damageHandler != null && damageHandler.InHurtState;
        spriteRenderer.color = isInHurtState
            ? new Color(1, Mathf.PingPong(Time.time * 3, 1), Mathf.PingPong(Time.time * 3, 1))
            : new Color(1, 1, 1);

        if (_pursueTarget && _controlledMover)
        {
            StartCoroutine(_controlledMover.WalkTo(_pursueTarget.position, false));
        }
        else
        {
            AttachPlayerTarget();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
