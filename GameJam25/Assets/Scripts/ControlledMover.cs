using System.Collections;
using System.Threading;
using UnityEngine;

public class ControlledMover : MonoBehaviour
{
    enum MovementType { None, Floating, Walking }

    [SerializeField] float WalkingSpeed = 6;
    [SerializeField] float FloatingSpeed = 6;

    Transform _gfxHolder;

    MovementType _movementType = MovementType.None;

    CancellationTokenSource _movementCancel = new();
    float _minTargetDistanceThreshold = 0.1f;
    Vector3 _targetPosition;

    void Awake()
    {
        _gfxHolder = transform.Find("Root").Find("GFX");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _targetPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _minTargetDistanceThreshold)
        {
            // Rotate GFX based on where they are going.
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
                // Update object to move towards target position.
                var speed = _movementType == MovementType.Walking ? WalkingSpeed : FloatingSpeed;
                var distance = speed * Time.deltaTime * Time.timeScale;
                //Debug.Log("Move distance " + distance);
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, distance);
            }
        }
    }

    void CancelMovement()
    {
        _movementCancel.Cancel();
        _movementCancel = new CancellationTokenSource();
    }

    void SetMovementTarget(Vector3 targetPos, MovementType movementType)
    {
        Debug.Log(gameObject.name + " SetMovementTarget " + targetPos + " " + movementType);

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
        Debug.Log(gameObject.name + " snapped " + targetPos);

        transform.position = targetPos;
        SetMovementTarget(new Vector3(targetPos.x, targetPos.y, targetPos.z), MovementType.None);
    }

    public IEnumerator FloatTo(Vector3 targetPos)
    {
        Debug.Log(gameObject.name + " floating down " + targetPos);

        SetMovementTarget(targetPos, MovementType.Floating);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log(gameObject.name + " landed");
        }
    }

    public IEnumerator WalkTo(Vector2 targetPos)
    {
        Debug.Log(gameObject.name + " walking " + targetPos);

        SetMovementTarget(targetPos, MovementType.Walking);
        yield return WaitUntilCloseToTarget();

        if (!_movementCancel.IsCancellationRequested)
        {
            Debug.Log(gameObject.name + " stopped");
        }
    }
}
