using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    [SerializeField] float ShakeAmount = 1;

    float _shakeSecondsRemaining;

    public void Follow(GameObject o)
    {
        Debug.Log("Camera following " + o.name);
        //Player = o.GetComponent<Transform>(); // Welp, I thought this would work, but it does not. --ECarter
    }

    public IEnumerator Shake(float seconds)
    {
        _shakeSecondsRemaining = seconds;
        Debug.Log("Camera shake");
        yield return new WaitWhile(() => _shakeSecondsRemaining > 0);
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, new RefreshRate() { numerator = 1, denominator = 60 });
    }

    void Update()
    {
        if (_shakeSecondsRemaining > 0)
        {
            transform.parent.localPosition = UnityEngine.Random.insideUnitSphere * ShakeAmount;
            _shakeSecondsRemaining -= Time.deltaTime * Time.timeScale;
            if (_shakeSecondsRemaining <= 0)
            {
                transform.parent.localPosition = new Vector3();
                _shakeSecondsRemaining = 0;
            }
        }
    }
}
