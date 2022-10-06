using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumb : MonoBehaviour
{

    private GameManager gameManager;
    private EnemyMove enemyMove;

     void Start()
    {
        gameManager = GetComponent<GameManager>();
        enemyMove = GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
