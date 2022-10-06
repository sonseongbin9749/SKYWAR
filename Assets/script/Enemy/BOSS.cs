using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSS : MonoBehaviour
{
    [SerializeField]
    private int HP = 1000;
    [SerializeField]
    private Transform bossbulletPosition1;
    [SerializeField]
    private Transform bossbulletPosition2;
    [SerializeField]
    private Transform bossbulletPosition3;
    [SerializeField]
    private Transform bossbulletPosition4;
    [SerializeField]
    private GameObject BossBulletPrefab;

    
 


    private Animator animator;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private bool isDead = false;
    
    public int enemyhp = 5;
    [SerializeField]
    private int patternindex;
    [SerializeField]

    private int currentPatterncount;
    [SerializeField]
    private int[] maxPatterncount;







    private void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
            
      
        
       
        enemyhp = HP;
        
        Debug.Log("앙");
        if (enemyhp == 4000)
        {
            Debug.Log("기모띠");
            
            
                Invoke("STOP", 5);
            
        }
    }


  

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) { return; }
        if (collision.tag == "Bullet")
        {
            StartCoroutine(DamageEffect());
            enemyhp--;

            collision.GetComponent<BulletPosition>().Despawn();


            if (enemyhp <= 0)
            {
                isDead = true;
                col.enabled = false;
                animator.Play("Explosion");
                PlayerPrefs.SetInt("HIGHSCORE", gameManager.Highscoreup());
                SceneManager.LoadScene("Gameclear");

            }



        }
        else if (collision.tag == ("misail"))
        {
            StartCoroutine(DamageEffect());
            enemyhp--;


            collision.GetComponent<misailMove>().Respawn();

            

        }

    }



    public IEnumerator DamageEffect()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.color = Color.white;

    }

    private void STOP()
    {
        Debug.Log("헤으응");
        if (!gameObject.activeSelf)
        {
            return;
        }
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;

        Invoke("THINK", 4);
    }

    void THINK()
    {
        

        Debug.Log("%기있");
        
        //patternindex = patternindex == 3 ? 0 : patternindex + 1;
        currentPatterncount = 0;
        patternindex = Random.Range(1, 4);
        
            switch (patternindex)
            {

                case 1:
                    {
                        FireFoward();

                        break;
                    }
                case 2:
                    {
                        FireShot();
                        Debug.Log("안녕하세요");
                        break;
                    }
                case 3:
                    {
                        FireArc();
                        Debug.Log("감사해요");
                        break;
                    }
            

            
                
                    
            }
            

        
    }

    void  FireFoward()
    {
        currentPatterncount++;
        int count = 0;
        GameObject bullet1, bullet2, bullet3, bullet4;
        
        while (count < 1)
        {
            
            bullet1 = Instantiate(BossBulletPrefab, bossbulletPosition1);
            bullet2 = Instantiate(BossBulletPrefab, bossbulletPosition2);
            bullet3 = Instantiate(BossBulletPrefab, bossbulletPosition3);
            bullet4 = Instantiate(BossBulletPrefab, bossbulletPosition4);




            bullet1.transform.SetParent(null);
            bullet2.transform.SetParent(null);
            bullet3.transform.SetParent(null);
            bullet4.transform.SetParent(null);
            count++;
        }
        

        if (currentPatterncount < maxPatterncount[patternindex])
        {
            Debug.Log("쏴라!!");  
            Invoke("FireFoward", 0.1f);
        }
        else
        {
            
            Invoke("THINK", 3);
        }
        //앞으로 4번 발사
    }
    void FireShot()
    {
        

        
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(BossBulletPrefab);
            bullet.transform.position = gameManager.BOSSPosition.position;
            bullet.transform.rotation = Quaternion.Euler(0f,0f,Random.Range(-20,20));
        }

        currentPatterncount++;

        if (currentPatterncount < maxPatterncount[patternindex])
        {
            Invoke("FireShot", 2.5f);

        }
        else
        {
            
            Invoke("THINK", 3);
        }
        //플레이어 방향으로 샷건
    }
    void FireArc()
    {
        currentPatterncount++;
        GameObject bullet = Instantiate(BossBulletPrefab);
        bullet.transform.position = gameManager.BOSSPosition.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(currentPatterncount), -1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        



        if (currentPatterncount < maxPatterncount[patternindex])
        {
            Invoke("FireArc", 0.5f);

        }
        else
        {
          
            Invoke("THINK", 3);
        }
        //부체 모양으로 발사
    }

    private void MOVE()
    {
        transform.Translate(Vector2.down * 0.3f);
        if (transform.localPosition.y < gameManager.MinPosition.y)
        {
            Destroy(gameObject);
        }
    }
    
}

