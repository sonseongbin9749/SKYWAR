using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Define;

public class MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 15000; //15000개의 파티클을 만들꺼다

    private int _quadIndex = 0; //현재 생성된 쿼드의 번호

    [Serializable]
    public struct ParticleUVPixel
    {
        public Vector2Int uv00Pixel; //좌측 하단 
        public Vector2Int uv11Pixel; //우측 상단
    }

    public struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField]
    private ParticleUVPixel[] _uvPixelArr;
    private UVCoords[] _uvCoordArr;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;

    private bool _updateVertices;
    private bool _updateUV;
    private bool _updateTriangles;

    private void Awake()
    {
        _mesh = new Mesh();
        
        _vertices = new Vector3[4 * MAX_QUAD_AMOUNT];
        _uv = new Vector2[4 * MAX_QUAD_AMOUNT];
        _triangles = new int[6 * MAX_QUAD_AMOUNT];

        _mesh.vertices = _vertices;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;

        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.sortingLayerName = "Agent";
        _meshRenderer.sortingOrder = 0;

        //여기서부터 pixel을 uv로 변환하는 작업을 한다.
        Texture mainTex = _meshRenderer.material.mainTexture;
        int tWidth = mainTex.width;
        int tHeight = mainTex.height;

        List<UVCoords> uvCoordList = new List<UVCoords>();

        foreach(ParticleUVPixel pixel in _uvPixelArr)
        {
            UVCoords temp = new UVCoords
            {
                uv00 = new Vector2( (float)pixel.uv00Pixel.x / tWidth, 
                                    (float)pixel.uv00Pixel.y / tHeight  ),
                uv11 = new Vector2((float)pixel.uv11Pixel.x / tWidth,
                                    (float)pixel.uv11Pixel.y / tHeight  )
            };
            uvCoordList.Add(temp);
        }
        _uvCoordArr = uvCoordList.ToArray(); 
        //배열로 변환해서 참조하면 속도가 조금 더 빨라
    }

    public int GetRandomBloodIndex()
    {
        return Random.Range(0, 8);
    }

    public int GetRandomShellIndex()
    {
        return Random.Range(8, 9); //무조건 8만 나옴
    }


    public int AddQuad(Vector3 position, float rotation, Vector3 quadSize, 
                                        bool skewed, int uvIndex)
    {
        UpdateQuad(_quadIndex, position, rotation, quadSize, skewed, uvIndex);

        int spawnedQuadIndex = _quadIndex;
        _quadIndex = (_quadIndex + 1) % MAX_QUAD_AMOUNT; //최대치를 초과하면 0번째로

        return spawnedQuadIndex;
    }

    public void UpdateQuad(int quadIndex, Vector3 position, float rotation,
                                    Vector3 quadSize, bool skewed, int uvIndex)
    {
        int vIndex0 = quadIndex * 4;
        int vIndex1 = vIndex0 + 1;
        int vIndex2 = vIndex0 + 2;
        int vIndex3 = vIndex0 + 3;

        if(skewed)
        {
            _vertices[vIndex0] = position + 
                Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
            _vertices[vIndex1] = position +
                Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
            _vertices[vIndex2] = position +
                Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, +quadSize.y);
            _vertices[vIndex3] = position +
                Quaternion.Euler(0, 0, rotation) * new Vector3(+quadSize.x, -quadSize.y);
        }
        else
        {
            _vertices[vIndex0] = position + 
                Quaternion.Euler(0, 0, rotation - 180) * quadSize; // -1, -1
            _vertices[vIndex1] = position +
                Quaternion.Euler(0, 0, rotation - 270) * quadSize; // -1, 1
            _vertices[vIndex2] = position +
                Quaternion.Euler(0, 0, rotation - 0) * quadSize; // 1, 1
            _vertices[vIndex3] = position +
                Quaternion.Euler(0, 0, rotation - 90) * quadSize; // 1, -1
        }

        /*
         * 1(-270) -- 2 (0도)
         * |          |
         * 0(-180) -- 3 (-90도)
         */

        UVCoords uv = _uvCoordArr[uvIndex];  //랜덤하게 받은 uv를 뽑아와서
        _uv[vIndex0] = uv.uv00;
        _uv[vIndex1] = new Vector2(uv.uv00.x, uv.uv11.y);
        _uv[vIndex2] = uv.uv11;
        _uv[vIndex3] = new Vector2(uv.uv11.x, uv.uv00.y);

        int tIndex = quadIndex * 6;
        _triangles[tIndex + 0] = vIndex0;
        _triangles[tIndex + 1] = vIndex1;
        _triangles[tIndex + 2] = vIndex2;

        _triangles[tIndex + 3] = vIndex0;
        _triangles[tIndex + 4] = vIndex2;
        _triangles[tIndex + 5] = vIndex3;


        //변경이 생겼을 때 쫌 무식하게 그냥 넣어준다.

        _updateVertices = true;
        _updateUV = true;
        _updateTriangles = true;
    }

    private void LateUpdate()
    {
        if(_updateVertices)
        {
            _mesh.vertices = _vertices;
            _updateVertices = false;
        }
        if (_updateUV)
        {
            _mesh.uv = _uv;
            _updateUV = false;
        }
        if (_updateTriangles)
        {
            _mesh.triangles = _triangles;
            _updateTriangles = false;
        }
    }

    public void DestroyQuad(int idx)
    {
        int vIndex0 = idx * 4;
        int vIndex1 = vIndex0 + 1;
        int vIndex2 = vIndex0 + 2;
        int vIndex3 = vIndex0 + 3;

        _vertices[vIndex0] = Vector3.zero;
        _vertices[vIndex1] = Vector3.zero;
        _vertices[vIndex2] = Vector3.zero;
        _vertices[vIndex3] = Vector3.zero;

        _updateVertices = true;
    }

    public void DestroyAllQuad()
    {
        Array.Clear(_vertices, 0, _vertices.Length); //싹다 0으로 초기화
        _quadIndex = 0;
        _updateVertices = true;
    }
}
