using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnStartScene : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameManager gameManager;
    
    void Start()
    {
        StartCoroutine(Scene());
    }

    
    void Update()
    {
        
    }
    private IEnumerator Scene()
    {
        animator.Play("Start");
        yield return new WaitForSeconds(4f);
        gameManager.NEXT();
    }
    
}
