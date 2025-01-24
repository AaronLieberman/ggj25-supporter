using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSpriteSwitch : MonoBehaviour
{
    private bool _filled = true;
    public bool Filled
    {
        get { return _filled; }
        set
        {
            _filled = value;
            UpdateGraphic();
        }
    }

    public Sprite FilledSprite;
    public Sprite EmptySprite;

    private Image _sprite;
    private Animator _animator;

    private void Awake()
    {
        _sprite = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        _sprite.sprite = Filled ? FilledSprite : EmptySprite;
        if (Filled)
        {
            _animator.Play("Throb");
        }
        else
        {
            _animator.Play("Empty");
        }
    }
}
