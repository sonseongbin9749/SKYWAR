using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    private Vector3 _quadPosition;
    private Vector3 _direction;
    private MeshParticleSystem _meshPS;
    private int _quadIndex;
    private Vector3 _quadSize;
    private float _rotation;
    private int _uvIndex;

    private float _moveSpeed;
    private float _slowDownFactor;

    private bool _isRotate;

    public int QuadIndex {get => _quadIndex;}

    public Particle(Vector3 pos, Vector3 direction, MeshParticleSystem meshPs, 
        int uvIndex, float moveSpeed, Vector3 quadSize, float slowDownFactor, 
        bool isRotate = false)
    {
        _quadPosition = pos;
        _direction = direction;
        _meshPS = meshPs;
        _isRotate = isRotate;
        _quadSize = quadSize;

        _rotation = Random.Range(0, 360f);
        _uvIndex = uvIndex;
        _moveSpeed = moveSpeed;
        _slowDownFactor = slowDownFactor;

        _quadIndex = _meshPS.AddQuad(_quadPosition, _rotation, _quadSize, true, _uvIndex);
    }

    //외부의 매니저가 이걸 실행시켜줄꺼야
    public void Update()
    {
        _quadPosition += _direction * _moveSpeed * Time.deltaTime;

        if(_isRotate)
        {
            _rotation += 360f * (_moveSpeed * 0.1f) * Time.deltaTime;
        }
        _meshPS.UpdateQuad(_quadIndex, _quadPosition, _rotation, _quadSize, true, _uvIndex);

        _moveSpeed -= _moveSpeed * _slowDownFactor * Time.deltaTime;
    }

    public bool IsComplete()
    {
        return _moveSpeed < 0.05f;
    }
}
