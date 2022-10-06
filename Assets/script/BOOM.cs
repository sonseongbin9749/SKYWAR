using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM : MonoBehaviour
{
    private Collider2D col = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private Enemymiddleboss enemymiddlebossmove = null;
    private Enemy enemymove = null;
   
    void Start()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemymove = GetComponent<Enemy>();
        enemymiddlebossmove = GetComponent<Enemymiddleboss>();
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Player")
       {
            Destroy(enemymove.gameObject);
            Destroy(enemymiddlebossmove.gameObject);
       }
    }

}
