using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class MeshPositionMapBinder : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private int _index = 1;

    private VisualEffect _vfx = null;
    private SkinnedMeshRenderer _skinnedMeshRenderer = null;
    private MeshFilter _meshFilter = null;

    private Mesh _mesh = null;
    private Texture2D _bakedTex = null;

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();

        if (_target.TryGetComponent(out MeshFilter filter))
        {
            _mesh = filter.mesh;
        }

        if (_target.TryGetComponent(out _skinnedMeshRenderer))
        {
            _mesh = new Mesh();
        }
    }

    private void Update()
    {
        UploadMeshTexture();
    }

    private void UploadMeshTexture()
    {
        UpdateMesh();
        UpdatePositionMap();

        _vfx.SetTexture($"PositionMap{_index}", _bakedTex);
        _vfx.SetInt($"Size{_index}", _bakedTex.width);
        _vfx.SetMatrix4x4($"VolumeTransform{_index}", _target.transform.localToWorldMatrix);
    }

    private void UpdateMesh()
    {
        if (_skinnedMeshRenderer != null)
        {
            _skinnedMeshRenderer.BakeMesh(_mesh);
        }
    }

    private void UpdatePositionMap()
    {
        Vector3[] vertices = _mesh.vertices;
        int count = vertices.Length;

        float r = Mathf.Sqrt(count);
        var width = (int)Mathf.Ceil(r);
        var height = width;

        var positions = vertices.Select(vtx => new Color(vtx.x, vtx.y, vtx.z));

        if (_bakedTex == null || _bakedTex.width != width || _bakedTex.height != height)
        {
            Destroy(_bakedTex);
            
            _bakedTex = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
            _bakedTex.filterMode = FilterMode.Point;
            _bakedTex.wrapMode = TextureWrapMode.Clamp;
        }

        var buf = new Color[width * height];

        var idx = 0;
        foreach (var color in positions)
        {
            buf[idx] = color;
            idx++;
        }

        _bakedTex.SetPixels(buf);
        _bakedTex.Apply();
    }
}