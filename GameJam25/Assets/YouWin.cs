using UnityEngine;

public class YouWin : MonoBehaviour
{
    PlayerController _player;
    Canvas _canvas;

    private void Awake()
    {
        _player = Utilities.GetRootComponent<PlayerController>();
        _canvas = GetComponent<Canvas>();
    }

    public void ShowYouWin()
    {
        _canvas.enabled = true;
    }
}
