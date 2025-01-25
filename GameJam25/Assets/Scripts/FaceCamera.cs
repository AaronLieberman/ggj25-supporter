using UnityEngine;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
