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
    enum MovementType { Floating, Walking }

    [SerializeField] float MovementSpeed = 6;

    HeroState _heroState = HeroState.Idle;
    MovementType _movementState = MovementType.Floating;

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

    CancellationTokenSource _movementCancel = new();
    float _minTargetDistanceThreshold = 0.1f;
    Vector2 _targetPosition;

    bool _isDivingGearEquipped = false;

    void Start()
    {
        EntityResources = GetComponent<EntityResources>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _gfxHolder = transform.Find("GFX");
        _dialogBubble = GetComponentInChildren<DialogBubbleController>();

        _targetPosition = transform.position;
    }

    void Update()
    {
        if (Math.Abs(Vector2.Distance(transform.position, _targetPosition)) > _minTargetDistanceThreshold)
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
                transform.position.x > _targetPosition.x ? -1 : _gfxHolder.localScale.x * _gfxHolder.localScale.x,
                _gfxHolder.localScale.y,
                _gfxHolder.localScale.z
            );
        }

        // Update hero to move towards target position.
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, MovementSpeed * Time.deltaTime * Time.timeScale);
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

    void SetMovementTarget(Vector2 targetPos, MovementType movementType)
    {
        _movementState = movementType;
        _targetPosition = targetPos;
        CancelMovement();
    }

    bool IsCloseToTarget()
    {
        return Math.Abs(Vector2.Distance(transform.position, _targetPosition)) < _minTargetDistanceThreshold;
    }

    IEnumerator WaitUntilCloseToTarget()
    {
        yield return new WaitUntil(() => IsCloseToTarget() || _movementCancel.IsCancellationRequested);
    }

    public IEnumerator FloatTo(Vector2 targetPos)
    {
        Debug.Log("Hero floating down");

        SetMovementTarget(targetPos, MovementType.Floating);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log("Hero landed");
        }
    }

    public IEnumerator WalkTo(Vector2 targetPos)
    {
        Debug.Log("Hero walking");

        SetMovementTarget(targetPos, MovementType.Walking);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log("Hero stopped");
        }
    }

    public IEnumerator Say(string v)
    {
        Debug.Log("Hero says: \"" + v + "\"");
        return _dialogBubble.PopDialog(v);
    }

    public IEnumerator Throw(string itemName, Vector2 targetPos)
    {
        Debug.Log("Hero throws " + itemName);
        yield return Utilities.WaitForSeconds(1);
    }

    public void SetDivingGear(bool v)
    {
        _isDivingGearEquipped = v;
    }
}
