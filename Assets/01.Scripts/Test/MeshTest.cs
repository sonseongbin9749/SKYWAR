using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    private MeshFilter _meshFilter = null;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        // 스페이스바 누르면 랜덤한 핏자국중 하나가 나오도록 해봐
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DrawBlood();
        }
    }

    private void DrawBlood()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];


        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 10);
        vertices[2] = new Vector3(10, 10);
        vertices[3] = new Vector3(10, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        int idx = Random.Range(0, 8);
        float width = 1 / 8f;

        uv[0] = new Vector2(width * idx, 0.5f);
        uv[1] = new Vector2(width * idx, 1f);
        uv[2] = new Vector2(width * (idx+1) , 1f);
        uv[3] = new Vector2(width * (idx + 1), 0.5f);


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _meshFilter.mesh = mesh;
    }
}
