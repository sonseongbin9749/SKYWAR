using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private int _healthAmountPerSeparator = 2; //체력 2당 바 한개
    [SerializeField]
    private float _barSize = 1.88f;
    [SerializeField]
    private Vector3 _sepSize = new Vector3(0.02f, 0.5f);

    private Transform _barTrm;
    private Transform _separator;

    private int _maxHealth = -1;
    private int _health = -1;

    private MeshFilter _sepMeshFilter;
    private Mesh _sepMesh;
    private MeshRenderer _sepMeshRenderer;

    private void Start()
    {
        _barTrm = transform.Find("Bar");
        _barTrm.localScale = new Vector3(0, 1f, 1);

        _separator = transform.Find("SeparatorContainer/Separator");
        _sepMeshFilter = _separator.GetComponent<MeshFilter>();
        _sepMeshRenderer = _separator.GetComponent<MeshRenderer>();

        _sepMeshRenderer.sortingLayerName = "TOP";
        _sepMeshRenderer.sortingOrder = 25;

        //gameObject.SetActive(false);
    }

    public void SetHealth(int health)
    {
        if(_maxHealth < 0)
        {
            //gameObject.SetActive(true);
            _maxHealth = health;
            CalcSeparator(health);
        }
        _health = health;
        _barTrm.DOScaleX((float)_health / _maxHealth, 0.3f);
    }

    private void CalcSeparator(int value)
    {
        _sepMesh = new Mesh();

        int gridCnt = Mathf.FloorToInt(value / _healthAmountPerSeparator);
        
        Vector3[] vertices = new Vector3[(gridCnt - 1) * 4];
        Vector2[] uv = new Vector2[(gridCnt - 1) * 4];
        int[] triangles = new int[(gridCnt - 1) * 6];

        float barOneSize = _barSize / gridCnt;

        for(int i = 0; i < gridCnt - 1; i++)
        {
            Vector3 pos = new Vector3(barOneSize * (i + 1), 0, 0);

            int vIndex = i * 4;
            vertices[vIndex + 0] = pos + new Vector3(-_sepSize.x, -_sepSize.y);
            vertices[vIndex + 1] = pos + new Vector3(-_sepSize.x, +_sepSize.y);
            vertices[vIndex + 2] = pos + new Vector3(+_sepSize.x, +_sepSize.y);
            vertices[vIndex + 3] = pos + new Vector3(+_sepSize.x, -_sepSize.y);

            uv[vIndex + 0] = Vector2.zero;
            uv[vIndex + 1] = Vector2.up;
            uv[vIndex + 2] = Vector2.one;
            uv[vIndex + 3] = Vector2.right;

            int tIndex = i * 6;
            triangles[tIndex + 0] = vIndex + 0;
            triangles[tIndex + 1] = vIndex + 1;
            triangles[tIndex + 2] = vIndex + 2;
            triangles[tIndex + 3] = vIndex + 0;
            triangles[tIndex + 4] = vIndex + 2;
            triangles[tIndex + 5] = vIndex + 3;
        }
        _sepMesh.vertices = vertices;
        _sepMesh.uv = uv;
        _sepMesh.triangles = triangles;

        _sepMeshFilter.mesh = _sepMesh;
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.H))
    //    {
    //        SetHealth(40);
    //    }

    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        SetHealth(5);
    //    }
    //}
}
