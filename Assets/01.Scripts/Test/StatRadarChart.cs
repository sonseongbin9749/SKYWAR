using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRadarChart : MonoBehaviour
{
    private Stats _stats;
    private CanvasRenderer _radarMeshCanvasRenderer;
    [SerializeField] private Material _radarMat;

    private void Awake()
    {
        _radarMeshCanvasRenderer = transform.Find("RadarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats)
    {
        this._stats = stats;
        _stats.OnStatChanged += UpdateStatsVisual;
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        float radarChartSize = 145f; //최대높이 쟤봐야 한다. 
        float angleInc = 360f / 5;

        Vector3 attackVertex = Quaternion.Euler(0, 0, -angleInc * 0) 
                                * Vector3.up 
                                * radarChartSize 
                                * _stats.GetStatAmountNormalized(Stats.Type.Attack);
        int attackVertexIndex = 1;

        Vector3 defenceVertex = Quaternion.Euler(0, 0, -angleInc * 1)
                                * Vector3.up
                                * radarChartSize
                                * _stats.GetStatAmountNormalized(Stats.Type.Defence);
        int defenceVertexIndex = 2;


        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        vertices[0] = Vector3.zero;
        vertices[attackVertexIndex] = attackVertex;
        vertices[defenceVertexIndex] = defenceVertex;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _radarMeshCanvasRenderer.SetMesh(mesh);
        _radarMeshCanvasRenderer.SetMaterial(_radarMat, null); 
        //2번째꺼는 텍스쳐다. 지금은 그냥 연두색으로만
    }
}
