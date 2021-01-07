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

    private BakeMeshToTexture CurrentBaker => _bakers[_index];

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

    private void Update()
    {
        CurrentBaker.UploadMeshTexture();
    }

    private void Change()
    {
        _index = (_index + 1) % _bakers.Length;

        _vfx.SetTexture($"PositionMap", CurrentBaker.BakedTexture);
        _vfx.SetInt($"Size", CurrentBaker.BakedTexture.width);
        _vfx.SetMatrix4x4($"VolumeTransform", CurrentBaker.Transform.localToWorldMatrix);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 130, 30), "Change"))
        {
            Change();
        }
    }
}