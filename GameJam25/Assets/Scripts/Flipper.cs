using UnityEngine;

public class Flipper : MonoBehaviour
{
    Vector3 _lastPosition;

    void Start()
    {
        _lastPosition = transform.position;
    }

    void LateUpdate()
    {
        // Rotate GFX based on where they are going.
        if (transform.position != _lastPosition)
        {
            var shouldFaceRight = transform.position.x >= _lastPosition.x;
            if (transform.localScale.x > 0 != shouldFaceRight)
            {
                transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            _lastPosition = transform.position;
        }
    }
}
