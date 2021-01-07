using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class BakeMeshToTexture : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;

    public Texture2D BakedTexture { get; private set; }
    public Transform Transform => _target.transform;

    private SkinnedMeshRenderer _skinnedMeshRenderer = null;
    private Mesh _mesh = null;
    private bool _isSkinnedMesh = false;
    private Color[] _colorBuffer = null;

    public void Initialize()
    {
        if (_target.TryGetComponent(out MeshFilter filter))
        {
            _mesh = filter.mesh;
        }

        if (_target.TryGetComponent(out _skinnedMeshRenderer))
        {
            _isSkinnedMesh = true;
            _mesh = new Mesh();
            BakeMesh();
        }

        Vector3[] vertices = _mesh.vertices;
        int count = vertices.Length;

        float r = Mathf.Sqrt(count);
        int size = (int)Mathf.Ceil(r);

        _colorBuffer = new Color[size * size];

        BakedTexture = new Texture2D(size, size, TextureFormat.RGBAFloat, false);
        BakedTexture.filterMode = FilterMode.Point;
        BakedTexture.wrapMode = TextureWrapMode.Clamp;

        UpdatePositionMap();
    }

    public void UploadMeshTexture()
    {
        if (_isSkinnedMesh)
        {
            BakeMesh();
            UpdatePositionMap();
        }
    }

    private void BakeMesh()
    {
        _skinnedMeshRenderer.BakeMesh(_mesh);
    }

    private void UpdatePositionMap()
    {
        int idx = 0;
        foreach (Vector3 vert in _mesh.vertices)
        {
            _colorBuffer[idx] = VectorToColor(vert);
            idx++;
        }

        BakedTexture.SetPixels(_colorBuffer);
        BakedTexture.Apply();
    }

    private Color VectorToColor(Vector3 v)
    {
        return new Color(v.x, v.y, v.z);
    }
}