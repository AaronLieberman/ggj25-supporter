using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [HideInInspector] public EntityResources EntityResources;

    enum HeroState { Idle, Running }

    [SerializeField] float MovementSpeed = 6;

    HeroState _heroState = HeroState.Idle;

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;

    TaskCompletionSource<bool> _movementTask = new TaskCompletionSource<bool>();
    float _minTargetDistanceThreshold = 0.1f;
    Vector2 _targetPosition;

    void Start()
    {
        EntityResources = GetComponent<EntityResources>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        _targetPosition = transform.position;
    }

    void Update()
    {
        if (Math.Abs(Vector2.Distance(transform.position, _targetPosition)) > _minTargetDistanceThreshold)
        {
            MoveHero();
        }
        else if (!_movementTask.Task.IsCompleted)
        {
            _movementTask.SetResult(true);
        }
    }

    void MoveHero()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, MovementSpeed * Time.deltaTime);
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

    public void DashComplete()
    {
        // _heroState = HeroState.Idle;
        _rigidBody.linearVelocity = new Vector2(0.0f, 0.0f);
    }

    public IEnumerator FloatTo(Vector2 targetPos)
    {
        Debug.Log("Hero floating down");
        _targetPosition = targetPos;
        _movementTask = new TaskCompletionSource<bool>();
        yield return Utilities.DoAndWait(_movementTask.Task);
        Debug.Log("Hero landed");
    }

    public IEnumerator WalkTo(Vector2 targetPos)
    {
        Debug.Log("Hero walking");
        yield return Utilities.WaitForSeconds(1);
        Debug.Log("Hero stopped");
    }

    public IEnumerator Say(string v, float seconds)
    {
        Debug.Log("Hero says: \"" + v + "\"");
        yield return Utilities.WaitForSeconds(seconds);
    }

    public IEnumerator Throw(string itemName, Vector2 targetPos)
    {
        Debug.Log("Hero throws " + itemName);
        yield return Utilities.WaitForSeconds(1);
    }
}
