using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class MeshPositionMapBinder : MonoBehaviour
{
    [SerializeField] private BakeMeshToTexture[] _bakers = null;

    private VisualEffect _vfx = null;
    private int _index = -1;

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();

        foreach (var b in _bakers)
        {
            b.Initialize();
        }

        Change();
    }

    private void Change()
    {
        _index = (_index + 1) % _bakers.Length;
        _vfx.SetTexture($"PositionMap", _bakers[_index].BakedTexture);
        _vfx.SetInt($"Size", _bakers[_index].BakedTexture.width);
        _vfx.SetMatrix4x4($"VolumeTransform", _bakers[_index].Transform.localToWorldMatrix);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 130, 30), "Change"))
        {
            Change();
        }
    }
}