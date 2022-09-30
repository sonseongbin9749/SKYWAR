using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureParticleManager : MonoBehaviour
{
    public static TextureParticleManager Instance;
    private MeshParticleSystem _meshPS;
    private List<Particle> _bloodList;
    private List<Particle> _shellList;

    private void Awake()
    {
        _meshPS = GetComponent<MeshParticleSystem>();
        Instance = this;
        _bloodList = new List<Particle>();
        _shellList = new List<Particle>();
    }

    private void Update()
    {
        for(int i = 0; i < _bloodList.Count; i++)
        {
            Particle p = _bloodList[i];
            p.Update();
            if(p.IsComplete())
            {
                _bloodList.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < _shellList.Count; i++)
        {
            Particle p = _shellList[i];
            p.Update();
            if (p.IsComplete())
            {
                _shellList.RemoveAt(i);
                //_meshPS.DestroyQuad(p.QuadIndex); //이거 넣으면 탄피가 움직임을 멈췄을때 사라짐
                i--;
            }
        }
    }

    public void SpawnShell(Vector3 pos, Vector3 dir)
    {
        int uvIndex = _meshPS.GetRandomShellIndex();
        float moveSpeed = Random.Range(1.5f, 2.5f);
        Vector3 quadSize = new Vector3(0.15f, 0.15f);
        float slowDownFactor = Random.Range(2f, 2.5f);

        _shellList.Add(
            new Particle(pos, dir, _meshPS, uvIndex, moveSpeed, 
                        quadSize, slowDownFactor, true));
    }

    public void SpawnBlood(Vector3 pos, Vector3 dir)
    {
        int uvIndex = _meshPS.GetRandomBloodIndex();
        float moveSpeed = Random.Range(0.3f, 0.5f);
        float sizeFactor = Random.Range(0.3f, 0.8f);
        Vector3 quadSize = new Vector3(1f, 1f) * sizeFactor;
        float slowDownFactor = Random.Range(0.8f, 1.5f);

        _bloodList.Add(
            new Particle(pos, dir, _meshPS, uvIndex, moveSpeed, quadSize, slowDownFactor));
    }

    public void ClearAllParticle()
    {
        _meshPS.DestroyAllQuad(); //모든 쿼드 지워주고 다음방으로 이동할때 사용
    }
    
}
