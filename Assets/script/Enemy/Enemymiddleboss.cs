using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemymiddleboss : EnemyMove
{
    [SerializeField]
    private float speed = 1f;
   
    

   
   

   

    [SerializeField]
    private Transform bulletPosition;
    [SerializeField]
    private Transform bulletPosition2;
    [SerializeField]
    private GameObject bulletPrefab;
    
   

    
   
    private Vector2 direction;
    
    


    protected override void Start()
    {
        base.Start();
        StartCoroutine(Fire());
    }


    protected void Update()
    {
        transform.Translate(direction);
        Move();
    }

    private void Move()
    {
        transform.Translate(hellomyfreand1 * speed * Time.deltaTime);
        if (transform.localPosition.y < gameManager.MinPosition.y)
        {
            Destroy(gameObject);
        }
    }


    private IEnumerator Fire()
    {
        while (true)
        {
            Spawnbullet();
            
            yield return new WaitForSeconds(2f);
        }

    }

   

   

    private void Spawnbullet()
    {
        GameObject bullet, bullet1;
        bullet = Instantiate(bulletPrefab, bulletPosition);
        bullet1 = Instantiate(bulletPrefab, bulletPosition2);

        bullet.transform.SetParent(null);
        bullet1.transform.SetParent(null);
    }

}
