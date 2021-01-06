using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    [SerializeField] private Transform _origin = null;
    [SerializeField] private float _distance = 4f;
    [SerializeField] private float _height = 1.05f;
    [SerializeField] private float _speed = 1f;
    
    private void Start()
    {
        if (_origin == null)
        {
            _origin = new GameObject("Origin").transform;
        }
    }

    private void Update()
    {
        float t = Time.time * _speed * Mathf.Deg2Rad;
        float x = Mathf.Cos(t) * _distance;
        float z = Mathf.Sin(t) * _distance;
        
        Vector3 pos = new Vector3(x, _height, z);
        pos += _origin.transform.position;

        transform.position = pos;
        transform.LookAt(_origin);
    }
}
