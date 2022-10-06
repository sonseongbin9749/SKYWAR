using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybulletmove : MonoBehaviour
{
    GameObject target;
    
    [SerializeField] 
    float speed = 2f, rotSpeed = 2f;

    Quaternion rotTarget;
    Vector3 dir;
    Rigidbody2D rb;
    private GameManager gameManager;

    

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.Find("player");

        Guidedbullet();
    }


    void Update()
    {
        if (transform.localPosition.y < gameManager.MinPosition.y-2 || transform.localPosition.y > gameManager.MaxPosition.y+2)
        {
            Destroy(gameObject);
        }
        if(transform.localPosition.x < gameManager.MinPosition.x-2 || transform.localPosition.x > gameManager.MaxPosition.x+2)
        {
            Destroy(gameObject);
        }
    }

    void Guidedbullet()
    {
        
        
            dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * rotSpeed);
            rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
       


    }




}
