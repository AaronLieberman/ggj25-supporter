using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    PlayerController _player;
    Canvas _canvas;

    private void Awake()
    {
        _player = Utilities.GetRootComponent<PlayerController>();
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.enabled = false;

        _player.EntityResources.Death += ShowGameOver;
    }

    public void ShowGameOver()
    {
        _canvas.enabled = true;
    }
}
