using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{ 
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }
   
    [SerializeField]
    private Transform[] spawmpoints;
    [SerializeField]
    private GameObject enemyflight1;
    [SerializeField]
    private GameObject enemyflight2;
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text highscoreText = null;
    [SerializeField]
    public Transform BOSSPosition;
    [SerializeField]
    private GameObject BOSSprefab;
    [SerializeField]
    private GameObject MenuSet;
    [SerializeField]
    private Text DelaytimeText = null;

   [SerializeField]

   private AudioSource flightmusic;


    private SpriteRenderer spriteRenderer;
    public Poolbullet poolbullet { get; private set; }
    public Poolenemybullet poolenemybullet { get; private set; }
    public Poolenemy poolenemy { get; private set; }
    public Poolmisail poolmisail { get; private set; }
    

    public enemybulletmove enemyBulletmove { get; private set; }

    public PoolManager poolManager { get; private set; }
    public Playermove  player { get; private set; }

   


    private Playermove playermove = null;
    private Enemy enemyMove = null;
    private Enemymiddleboss enemymiddlebossmove = null;
    Vector3 target = new Vector3(1, 1.5f, 0);

    private int score = 0;
    private int highscore = 0;
    public int life = 3;
 
    public void ScoreUp(int upScore)
    {
        score += upScore;
    }

    public int Highscoreup()
    {
        return highscore;
    }
    void Start()
    {
       
        poolbullet = FindObjectOfType<Poolbullet>();
        enemyMove = FindObjectOfType<Enemy>();
        enemyBulletmove = GetComponent<enemybulletmove>();
        poolmisail = FindObjectOfType<Poolmisail>();
        
        MinPosition = new Vector2(-3f, -9.5f);
        
        MaxPosition = new Vector2(3f, 3f);
        enemymiddlebossmove = GetComponent<Enemymiddleboss>();
        playermove = GetComponent<Playermove>();
        StartCoroutine(Spawnenemy());
        StartCoroutine(Spawnmiddleboss());
        StartCoroutine(spawnBOSS());
        highscore = PlayerPrefs.GetInt("HIGHSCORE");
        UpdateUI();
        DelaytimeText.enabled = false;



    }

    void Update()
    {
        

        ang();
        
    }

    public void ang()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            flightmusic.Pause();
            MenuSet.SetActive(true);
            Time.timeScale = 0;
        }
    }


    public void UpdateScore(int addscore)
    {
        score += addscore;
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = string.Format("SCORE\n {0}", score);
        highscoreText.text = string.Format("HIGHSCORE\n{0}", highscore);
        if (score >= highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("HIGHSCORE", highscore);
            highscoreText.text = string.Format("HIGHSCORE\n{0}", highscore);
        }
    }

    private IEnumerator ContinueDelay()
    {
        int countTime = 3;

       
        while (countTime > 0)
        {
            DelaytimeText.text = string.Format("{0}", countTime);
            countTime--;
            yield return new WaitForSecondsRealtime(1f);

            if (countTime == 0)
                DelaytimeText.text = string.Format("");
        }

        Time.timeScale = 1f;
        
    }


    public void OnClickContinue()
    {

        DelaytimeText.enabled = true;
        MenuSet.SetActive(false);
        flightmusic.Play();
        StartCoroutine(ContinueDelay());

    }

    public void OnClickRealContinue()
    {
        Time.timeScale = 1f;
        flightmusic.Play();
        MenuSet.SetActive(false);
    }




    private IEnumerator Spawnenemy()
    { 
        float spawnDelay = 0f;
        yield return new WaitForSeconds(3f);
        while (true)
        {

            spawnDelay = Random.Range(3f, 5f);
            int ranPoint = Random.Range(0, 7);

            Rigidbody2D rigid = enemyflight1.GetComponent<Rigidbody2D>();
            
            
            for (int i = 0; i < 7; i++)
            {
               


   
                enemyMove = Instantiate(enemyflight1, spawmpoints[ranPoint].position, spawmpoints[ranPoint].rotation).GetComponent<Enemy>();
                if (ranPoint == 4)
                { 
                    enemyMove.SetDirection(new Vector2(1f, -0.4f));

                }
                else if (ranPoint == 5)
                {
                    enemyMove.SetDirection(new Vector2(-1f, -0.4f));
                }



                yield return new WaitForSeconds(0.2f);

            }

           

            yield return new WaitForSeconds(spawnDelay);


        }
    }

    

    private IEnumerator Spawnmiddleboss()
    {
        float spawnDelay1 = 0f;
        yield return new WaitForSeconds(3f);
        while (true)
        {
            
            spawnDelay1 = Random.Range(7f, 9f);
            int ranPoint1 = Random.Range(4, 6);

            Rigidbody2D rigid = enemyflight2.GetComponent<Rigidbody2D>();
           


            for (int i = 0; i < 1; i++)
            {
                enemymiddlebossmove = Instantiate(enemyflight2, spawmpoints[ranPoint1].position, spawmpoints[ranPoint1].rotation).GetComponent<Enemymiddleboss>();
                if (ranPoint1 == 4)
                {
                    enemymiddlebossmove.hellomyfreand1 = new Vector2(0.5f, -0.2f);
                }
                else if (ranPoint1 == 5)
                {
                    enemymiddlebossmove.hellomyfreand1 = new Vector2(-0.5f, -0.2f);
                }
            }

           

            yield return new WaitForSeconds(0.1f);

            

            yield return new WaitForSeconds(spawnDelay1);


        }
    }

    private IEnumerator spawnBOSS()
    {
        int spawncount = 0;
        int count = 0;
        Rigidbody2D rigid = BOSSprefab.GetComponent<Rigidbody2D>();
        while (true)
        {


            if(score == 6000)
            {
                Instantiate(BOSSprefab, BOSSPosition);
                spawncount++;
                if(spawncount == 1)
                {
                    break;
                }
            }
            count++;
            yield return new WaitForSeconds(0.1f);
            
        }
        
      
    }



   

   

    public void NEXT()
    {
        SceneManager.LoadScene("Main");
    }
}
 