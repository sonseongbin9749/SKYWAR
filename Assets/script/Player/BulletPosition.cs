using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPosition : MonoBehaviour
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

    public void Despawn()
    {
        transform.SetParent(gameManager.poolbullet.transform, false);
        gameObject.SetActive(false);
    }
    protected void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.localPosition.y > gameManager.MaxPosition.y)
        {
            Despawn();
        }
    }
}

