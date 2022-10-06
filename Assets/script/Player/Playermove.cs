using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playermove : MonoBehaviour
{
    
    
    private GameManager gameManager;
   

    private Animator animator;
    private Vector2 targetPosition = Vector2.zero;
    private Item item;
    private BOOM boom = null;


    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    public int power1 = 1;
    private int maxpower = 3;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject misailPrefab;
    [SerializeField]
    private Transform bulletPosition1;
    [SerializeField]
    private Transform bulletPosition2;
    [SerializeField]
    private Transform bulletPosition3;
    [SerializeField]
    private Transform bulletPosition4;
    [SerializeField]
    private Transform misailPosition;
    [SerializeField]
    private GameObject bumbEffect;
    public Poolbullet poolbullet { get; private set; }
    public Poolmisail poolmisail { get; private set; }

    [SerializeField]
    private GameObject[] images;

    private int cur = 2;



    private Collider2D col = null;
    private SpriteRenderer spriteRenderer = null;
    private Enemy enemy = null;
    


    




    private void Start()
    {
        poolbullet = FindObjectOfType<Poolbullet>();
        poolmisail = FindObjectOfType<Poolmisail>();
        gameManager = FindObjectOfType<GameManager>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        boom = GetComponent<BOOM>();
        col.enabled = true;
      
        gameManager.UpdateUI();
        StartCoroutine(Fire());
    }

    private void Update()
    {
        Move();
        
    }

    void Move()
    {

    
            if (Input.GetMouseButton(0) == true)
            {
            
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = Mathf.Clamp(targetPosition.x, gameManager.MinPosition.x, gameManager.MaxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, gameManager.MinPosition.y, gameManager.MaxPosition.y);

                transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
                

            }
        
        
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            Spawnbullet();
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void Bullet1()
    {
        GameObject bullet; 
        if(poolbullet.transform.childCount > 30)
        {
           
            bullet = poolbullet.transform.GetChild(0).gameObject;
            bullet.transform.SetParent(null);
            bullet.transform.position = bulletPosition1.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab, bulletPosition1);
            bullet.transform.SetParent(null);
        }
    }
    private void Bullet2()
    {
        GameObject bullet;
        if (poolbullet.transform.childCount > 30)
        {
            bullet = poolbullet.transform.GetChild(0).gameObject;
            bullet.transform.SetParent(null);
            bullet.transform.position = bulletPosition2.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab, bulletPosition2);
            bullet.transform.SetParent(null);
        }
    }

    private void Bullet3()
    {
        GameObject bullet;
        if (poolbullet.transform.childCount > 30)
        {
            bullet = poolbullet.transform.GetChild(0).gameObject;
            bullet.transform.SetParent(null);
            bullet.transform.position = bulletPosition3.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab, bulletPosition3);
            bullet.transform.SetParent(null);
        }
    }

    private void Bullet4()
    {
        GameObject bullet;
        if (poolbullet.transform.childCount > 30)
        {
            bullet = poolbullet.transform.GetChild(0).gameObject;
            bullet.transform.SetParent(null);
            bullet.transform.position = bulletPosition4.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab, bulletPosition4);
            bullet.transform.SetParent(null);
        }
    }
    private void Misail()
    {
        GameObject bullet;
        if (poolmisail.transform.childCount > 30)
        {
            bullet = poolmisail.transform.GetChild(0).gameObject;
            bullet.transform.SetParent(null);
            bullet.transform.position = misailPosition.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(misailPrefab, misailPosition);
            bullet.transform.SetParent(null);
        }
    }


    private void Spawnbullet()
    {         
       
        
        
            switch (power1)
            {
                case 1:
                {
                    Bullet2();
                    Bullet4();
                }
                    break;

                case 2:
                {
                    Bullet1();
                    Bullet2();
                    Bullet3();
                    Bullet4();
                }
                    break;

                case 3:
                {
                    Bullet1();
                    Bullet2();
                    Bullet3();
                    Bullet4();
                    Misail();
                }

                    break;
                default:
                {
                    Bullet1();
                    Bullet2();
                    Bullet3();
                    Bullet4();
                    Misail();
                }
                    break;
            }

        


    }

    

    private IEnumerator Damaged()
    {
        col.enabled = false;

        int i = 0;
        while( i < 5)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            i++;
        }

        col.enabled = true;
    }

     

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.life > 0)
        {
            if(collision.gameObject.layer == 6)
            {
                gameManager.life--;
                UpdateLifeIcon(gameManager.life);

                StartCoroutine(Damaged());
                power1 = 1;
                Debug.Log("¸Â¾Ò¶ì");
            }

            else if (collision.gameObject.tag == "item")
            {
                Item item = collision.gameObject.GetComponent<Item>();
                switch (item.type)
                {
                    case "COIN":
                        {
                            gameManager.ScoreUp(100);
                            gameManager.UpdateUI();
                            break;
                        }
                    case "POWER":
                        {
                            
                            if (power1 == maxpower)
                            
                            {
                                gameManager.ScoreUp(500);
                                power1++;
                                gameManager.UpdateUI();

                            }
                            else
                            {
                                power1++;
                            }
                            
                            break;
                        }
                    case "BOOM":
                        {
                           BOOM();
                           

                            Invoke("Offbumb", 1f);
                           
                            
                            break;
                        }


                }
                
                Destroy(collision.gameObject);





            }
            else if (collision.gameObject.tag == "Bossbullet")
            {
                gameManager.life--;
                UpdateLifeIcon(gameManager.life);

                StartCoroutine(Damaged());
                power1 = 1;
                Debug.Log("¸Â¾Ò¶ì");
            }


        }


        
        
        else if(gameManager.life == 0)
        {
            PlayerPrefs.SetInt("HIGHSCORE", gameManager.Highscoreup());
            SceneManager.LoadScene("Gameover");
        }
       
    }

    public void UpdateLifeIcon(int life)
    {
        
        images[cur].SetActive(false);
        cur--;  

    }


    private void BOOM()
    {

        bumbEffect.SetActive(true);
        



    }

    void Offbumb()
    {
        bumbEffect.SetActive(false);
    }

    public void PlayerStart()
    {
        animator.Play("Start");

    }
}
