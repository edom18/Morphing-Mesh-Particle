using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class BakePixelToTexture : MonoBehaviour
{
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private float _scale = 0.01f;
    [SerializeField] private int _divide = 2;

    public Texture2D PositionMap { get; private set; }
    public Texture2D ColorMap { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Initialize()
    {
        Width = _texture.width / _divide;
        Height = _texture.height / _divide;

        ColorMap = new Texture2D(Width, Height);
        Graphics.ConvertTexture(_texture, ColorMap);
        
        PositionMap = new Texture2D(Width, Height, TextureFormat.RGBAFloat, false);
        PositionMap.filterMode = FilterMode.Point;
        PositionMap.wrapMode = TextureWrapMode.Clamp;

        UpdatePositionMap();
    }

    private void UpdatePositionMap()
    {
        Color[] buffer = new Color[Width * Height];

        float halfWidth = Width * _scale * 0.5f;
        float halfHeight = Height * _scale * 0.5f;
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float px = (x * _scale) - halfWidth;
                float py = (y * _scale) - halfHeight;
                Vector3 v = new Vector3(px, py, 0);
                buffer[y * Width + x] = VectorToColor(v);
            }
        }

        PositionMap.SetPixels(buffer);
        PositionMap.Apply();
    }

    private Color VectorToColor(Vector3 v)
    {
        return new Color(v.x, v.y, v.z);
    }
}