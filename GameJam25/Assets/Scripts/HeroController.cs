using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [HideInInspector] public EntityResources EntityResources;

    enum HeroState { Idle, Running }
    enum MovementType { None, Floating, Walking }

    [SerializeField] float WalkingSpeed = 6;
    [SerializeField] float FloatingSpeed = 6;

    // HeroState _heroState = HeroState.Idle;
    MovementType _movementType = MovementType.None;

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;
    Transform _gfxHolder;
    DialogBubbleController _dialogBubble;
    Shadow _shadow;

    CancellationTokenSource _movementCancel = new();
    float _minTargetDistanceThreshold = 0.1f;
    Vector3 _targetPosition;

    bool _isDivingGearEquipped = false;

    private float lastSpeech;

    void Start()
    {
        EntityResources = GetComponent<EntityResources>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _gfxHolder = transform.Find("Root").Find("GFX");
        _dialogBubble = GetComponentInChildren<DialogBubbleController>();
        _shadow = GetComponentInChildren<Shadow>();

        _targetPosition = transform.position;
    }

    void Update()
    {
        var damageHandler = GetComponentInChildren<EntityDamageHandler>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        bool isInHurtState = damageHandler != null && damageHandler.InHurtState;

        if (isInHurtState)
        {
            spriteRenderer.color = new Color(
                1,
                Mathf.PingPong(Time.time * 3, 1),
                Mathf.PingPong(Time.time * 3, 1)
            );
        }
        else
        {
            spriteRenderer.color = new Color(
                1,
                1,
                1
            );
        }

        if (Vector3.Distance(transform.position, _targetPosition) > _minTargetDistanceThreshold)
        {
            MoveHero();
        }
    }

    void MoveHero()
    {
        // Rotate hero GFX based on where they are going.
        if (_gfxHolder)
        {
            _gfxHolder.localScale = new Vector3(
                _gfxHolder.position.x > _targetPosition.x ? -1 : _gfxHolder.localScale.x * _gfxHolder.localScale.x,
                _gfxHolder.localScale.y,
                _gfxHolder.localScale.z
            );
        }

        if (_movementType != MovementType.None)
        {
            // Update hero to move towards target position.
            var speed = _movementType == MovementType.Walking ? WalkingSpeed : FloatingSpeed;
            var distance = speed * Time.deltaTime * Time.timeScale;
            //Debug.Log("Move distance " + distance);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, distance);
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

    void CancelMovement()
    {
        _movementCancel.Cancel();
        _movementCancel = new CancellationTokenSource();
    }

    void SetMovementTarget(Vector3 targetPos, MovementType movementType)
    {
        Debug.Log("Hero SetMovementTarget " + targetPos + " " + movementType);

        CancelMovement();

        // if requesting walking, force to ground
        if (movementType == MovementType.Walking)
            transform.Translate(0, 0, -transform.position.z);

        _movementType = movementType;
        _targetPosition = targetPos;
    }

    bool IsCloseToTarget()
    {
        //Debug.Log("IsCloseToTarget " + transform.position + " " + _targetPosition + " " + Vector3.Distance(transform.position, _targetPosition));
        return Vector3.Distance(transform.position, _targetPosition) < _minTargetDistanceThreshold;
    }

    IEnumerator WaitUntilCloseToTarget()
    {
        yield return new WaitUntil(() => IsCloseToTarget() || _movementCancel.IsCancellationRequested);
    }

    public void SnapTo(Vector3 targetPos)
    {
        Debug.Log("Hero snapped " + targetPos);

        transform.position = targetPos;
        SetMovementTarget(new Vector3(targetPos.x, targetPos.y, targetPos.z), MovementType.None);
    }

    public IEnumerator FloatTo(Vector3 targetPos)
    {
        Debug.Log("Hero floating down " + targetPos);

        SetMovementTarget(targetPos, MovementType.Floating);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log("Hero landed");
        }
    }

    public IEnumerator WalkTo(Vector2 targetPos)
    {
        Debug.Log("Hero walking " + targetPos);

        SetMovementTarget(targetPos, MovementType.Walking);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log("Hero stopped");
        }
    }

    public IEnumerator Say(string v)
    {
        lastSpeech = Time.time;
        Debug.Log("Hero says: \"" + v + "\"");
        return _dialogBubble.PopDialog(v);
    }

    public IEnumerator Throw(string itemName, Vector2 targetPos)
    {
        Debug.Log("Hero throws " + itemName);
        yield return Utilities.WaitForSeconds(1);
    }

    public IEnumerator Brag()
    {
        while (true)
        {
            yield return Utilities.WaitForSeconds(1);
            if (Time.time > lastSpeech + 15)
            {
                Say("Hero_Brag");
            }
        }
    }

    public void SetDivingGear(bool v)
    {
        _isDivingGearEquipped = v;
    }

    public void startShootingMachineGun()
    {
        Debug.Log("TODO: Hero starts shooting machine gun");

        return;
    }

    internal void StartBragging()
    {
        StartCoroutine(Brag());
    }
}
