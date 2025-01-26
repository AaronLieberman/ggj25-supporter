using System.Collections.Generic;
using UnityEngine;

public class OctosharkController : MonoBehaviour
{
    [HideInInspector] public EntityResources EntityResources;

    enum OctosharkState { Idle, Running }

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    [SerializeField] float PursueInterval = 0.1f;
    float _pursueCooldownTimer = 0f;
    Transform _pursueTarget;

    ControlledMover _controlledMover;
    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;
    DialogBubbleController _dialogBubble;
    Shadow _shadow;

    bool _isDivingGearEquipped = false;

    private float lastSpeech;

    private void Awake()
    {
        EntityResources = GetComponent<EntityResources>();
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _dialogBubble = GetComponentInChildren<DialogBubbleController>();
        _shadow = GetComponentInChildren<Shadow>();
        _controlledMover = GetComponent<ControlledMover>();

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

    void PlayFootsteps()
    {
        _footstepTimer += Time.deltaTime;
        if (_footstepTimer >= FootstepInterval)
        {
            _footstepTimer = 0f;
            _audioSource.PlayOneShot(FootstepClips[UnityEngine.Random.Range(0, FootstepClips.Count - 1)], 0.5f);
        }
    }
}
