using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float Speed = 1;
    public float MaxLifetime = -1;
    public Vector3 Direction;
    float _startTime = 0;

    void Start()
    {
        if (Direction.sqrMagnitude > 0.0001f)
        {
            _startTime = Time.time;
        }
    }

    void Update()
    {
        if (GetComponent<Carriable>() && GetComponent<Carriable>().BeingCarried) return;
        if (_startTime == 0) return;

        transform.position += Speed * Time.deltaTime * Direction.normalized;

        if (MaxLifetime > 0 && Time.time > _startTime + MaxLifetime)
        {
            Destroy(gameObject);
        }
    }

    public void GoInDirection(Vector3 direction)
    {
        _startTime = Time.time;
        Direction = direction;
    }
}
