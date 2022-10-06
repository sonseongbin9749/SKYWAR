using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossbullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private GameManager gameManager;


    protected void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
    }

    protected void Update()
    {
        Move();
    }

  
    protected void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.localPosition.y < gameManager.MinPosition.y)
        {
            Destroy(gameObject);
        }
    }
}
