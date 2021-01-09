using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class PerlinNoiseEffect : MonoBehaviour
{
    [SerializeField] private BakePixelToTexture _baker = null;
    [SerializeField] private float _duration = 2f;

    private VisualEffect _vfx = null;

    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();
        _baker.Initialize();

        _vfx.SetTexture("PositionMap", _baker.PositionMap);
        _vfx.SetTexture("ColorMap", _baker.ColorMap);
        _vfx.SetInt("Width", _baker.Width);
        _vfx.SetInt("Height", _baker.Height);
        
        ResetParticle();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 130, 30), "Start"))
        {
            StartNoise();
        }
        
        if (GUI.Button(new Rect(10, 50, 130, 30), "Reset"))
        {
            ResetParticle();
        }
    }

    private void ResetParticle()
    {
        _vfx.SetFloat("Progress", 0);
        _vfx.SendEvent("OnPlay");
    }

    private void StartNoise()
    {
        StartCoroutine(StartNoiseCoroutine());
    }

    private IEnumerator StartNoiseCoroutine()
    {
        float time = 0;
        while (time <= _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;
            _vfx.SetFloat("Progress", t);
            yield return null;
        }
    }
}