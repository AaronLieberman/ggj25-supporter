using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    HeroController _hero;
    Canvas _canvas;

    private void Awake()
    {
        _hero = Utilities.GetRootComponent<HeroController>();
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.enabled = false;

        if (_hero && _hero.EntityResources) _hero.EntityResources.Death += ShowGameOver;
    }

    public void ShowGameOver()
    {
        _canvas.enabled = true;
    }
}
