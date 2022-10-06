using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    
    [SerializeField]
    protected GameObject itempower;
    [SerializeField]
    protected GameObject itemcoin;
    [SerializeField]
    protected GameObject itembumb;
    [SerializeField]
    protected int HP = 20;
    

    public int enemyhp = 5;
    public Vector2 hellomyfreand1 = Vector2.down;
    private Vector2 direction1;
    private SpriteRenderer spriteRenderer = null;
    private Collider2D col = null;
    private Animator animator;
    protected GameManager gameManager;
    public Playermove playermove;

    protected bool isDead = false;

    protected virtual void Start()
    {
        enemyhp = HP;
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) { return; }
        if (collision.tag == "Bullet")
        {
            StartCoroutine(DamageEffect());
            enemyhp--;
            
            collision.GetComponent<BulletPosition>().Despawn();


            //BulletPosition bullet = collision.GetComponent<BulletPosition>();
            //if (bullet != null)
            //{
            //    Destroy(gameObject);
            //}

            if (enemyhp <= 0)
            {
                isDead = true;
                col.enabled = false;
                animator.Play("Explosion");
                gameManager.UpdateScore(100);
                int ran = Random.Range(0, 15);
                if (ran <= 10)
                {
                }
                else if (ran <= 11)
                {
                    Instantiate(itembumb, transform.position, itembumb.transform.rotation);
                }
                else if (ran <= 13)
                {
                    Instantiate(itemcoin, transform.position, itemcoin.transform.rotation);
                }
                else if (ran <= 14)
                {
                    Instantiate(itempower, transform.position, itempower.transform.rotation);
                }
                Destroy(gameObject, 0.5f);
            }
        }
        else if (collision.tag == ("misail"))
        {
            StartCoroutine(DamageEffect());
            enemyhp--;
            
            collision.GetComponent<misailMove>().Respawn();
            if (enemyhp <= 0)
            {
                
                isDead = true;
                col.enabled = false;
                gameManager.UpdateScore(100);
                   
                animator.Play("Explosion");
                int ran = Random.Range(0, 15);
                if (ran <= 10)
                {
                    Debug.Log("NOITENG");
                }
                else if (ran <= 11)
                {
                    Instantiate(itembumb, transform.position, itembumb.transform.rotation);
                }
                else if (ran <= 13)
                {
                    Instantiate(itemcoin, transform.position, itemcoin.transform.rotation);
                }
                else if (ran <= 14)
                {
                    Instantiate(itempower, transform.position, itempower.transform.rotation);
                }
                Destroy(gameObject, 0.5f);
            }
            else
            {
                StartCoroutine(DamageEffect());
            }
        }
    }
    public IEnumerator DamageEffect()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.color = Color.white;

    }

    
}
