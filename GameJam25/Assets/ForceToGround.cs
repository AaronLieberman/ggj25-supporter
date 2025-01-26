using UnityEngine;

public class ForceToGround : MonoBehaviour
{
    void LateUpdate()
    {
        transform.Translate(0, 0, -transform.localPosition.z);
    }
}
