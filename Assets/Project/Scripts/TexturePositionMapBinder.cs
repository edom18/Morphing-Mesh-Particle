using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class TexturePositionMapBinder : MonoBehaviour
{
    [SerializeField] private BakePixelToTexture _baker = null;

    private VisualEffect _vfx = null;

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();
        _baker.Initialize();

        _vfx.SetTexture("PositionMap", _baker.PositionMap);
        _vfx.SetTexture("ColorMap", _baker.ColorMap);
        _vfx.SetInt("Width", _baker.Width);
        _vfx.SetInt("Height", _baker.Height);
    }
}