using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    [SerializeField] float PlayerOffset = 4;

    bool _firstFrame = true;
    float _initialY;

    public void Follow(GameObject o)
    {
        Debug.Log("Camera following " + o.name);
        //Player = o.GetComponent<Transform>(); // Welp, I thought this would work, but it does not. --ECarter
    }

    public IEnumerator Shake(float seconds)
    {
        Debug.Log("Camera shake");
        yield return Utilities.WaitForSeconds(seconds);
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, new RefreshRate() { numerator = 1, denominator = 60 });
    }

    void Update()
    {
        // do this the first frame because otherwise Player may not have been initialized yet
        if (Player)
        {
            if (_firstFrame)
            {
                _initialY = Player.transform.position.y;
                _firstFrame = false;
            }

            transform.position = new Vector3(
                Player.transform.position.x,
                _initialY + (Player.transform.position.y + PlayerOffset - _initialY) / 2,
                Player.transform.position.z - 10);
        }
    }
}
