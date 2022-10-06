using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyMove
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private Transform bulletPosition;
    [SerializeField]
    private GameObject bulletPrefab;
   
   
    private Vector2 hellomyfriend=Vector2.down;
    private Vector2 direction;

   
    public void SetDirection(Vector2 dir)
    {
        hellomyfriend = dir;
    }

    protected override void Start()
    {
        base.Start();
        Fire();
    }
    protected void Update()
    {
        if(isDead) { return; }
        Move();
        transform.Translate(direction);
       
    }

    private void Move()
    {
        transform.Translate(hellomyfriend * speed * Time.deltaTime);
        if (transform.localPosition.y < gameManager.MinPosition.y)
        {
            Destroy(gameObject);
        }
    }

    private void Fire()
    {
        GameObject bullet1;
        bullet1 = Instantiate(bulletPrefab, bulletPosition);
        bulletPrefab.transform.SetParent(null);

    }

   public float Speed()
    {
        return speed;
    }
}
