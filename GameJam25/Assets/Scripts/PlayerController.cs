using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance;
    public static PlayerController Instance => _instance;

    [HideInInspector] public EntityResources EntityResources;

    enum PlayerState { Idle, Running, Dashing }

    [SerializeField] float MovementSpeed = 6;
    [SerializeField] float DashSpeed = 16;

    PlayerState _playerState = PlayerState.Idle;

    [SerializeField] List<AudioClip> DashClips;
    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;

    void Start()
    {
        EntityResources = GetComponent<EntityResources>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var damageHandler = GetComponentInChildren<PlayerDamageHandler>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        bool isInHurtState = damageHandler != null && damageHandler.InHurtState;

        if (isInHurtState)
        {
            spriteRenderer.color = new Color(
                1,
                Mathf.PingPong(Time.time * 3, 1),
                Mathf.PingPong(Time.time * 3, 1)
            );
        } else
        {
            spriteRenderer.color = new Color(
                1,
                1,
                1
            );
        }

        if (EntityResources.isAlive && _playerState != PlayerState.Dashing)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
            {
                _spriteRenderer.flipX = horizontal < 0;
            }

            // some states are persistent until they complete, otherwise, default to idle unless we have a better
            // state to be in
            switch (_playerState)
            {
                case PlayerState.Dashing:
                    break;
                default:
                    _playerState = PlayerState.Idle;
                    break;
            }

            if (Input.GetButtonDown("Dash"))
            {
                _playerState = PlayerState.Dashing;

                _audioSource.PlayOneShot(DashClips[Random.Range(0, DashClips.Count - 1)]);
                _rigidBody.AddForce(new Vector2(DashSpeed * horizontal, (DashSpeed * vertical) * 0.5f), ForceMode2D.Impulse);
            }
            else if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                _playerState = PlayerState.Running;

                PlayFootsteps();
            }

            switch (_playerState)
            {
                case PlayerState.Idle:
                case PlayerState.Running:
                    _rigidBody.linearVelocity = new Vector2(horizontal * MovementSpeed, (vertical * MovementSpeed) * 0.5f);
                    break;
            }
        }

        _animator.SetInteger("state", (int)_playerState);
    }

    void PlayFootsteps()
    {
        _footstepTimer += Time.deltaTime;
        if (_footstepTimer >= FootstepInterval)
        {
            _footstepTimer = 0f;
            _audioSource.PlayOneShot(FootstepClips[Random.Range(0, FootstepClips.Count - 1)],0.5f);
        }
    }

    public void DashComplete()
    {
        _playerState = PlayerState.Idle;
        _rigidBody.linearVelocity = new Vector2(0.0f, 0.0f);
    }
}
