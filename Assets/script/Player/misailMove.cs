using UnityEngine;

public class misailMove : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private float speed = 10f;

    protected void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
    }

    protected void Update()
    {
        Move();
    }

   
    public void Respawn()
    {
        transform.SetParent(gameManager.poolmisail.transform, false);
        gameObject.SetActive(false);
    }
    protected void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.localPosition.y > gameManager.MaxPosition.y)
        {
            Respawn();
        }
    }
}
