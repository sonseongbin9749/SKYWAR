using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 생성")]
    [SerializeField]
    private List<EnemyDataSO> _enemyList;
    [SerializeField]
    private int _count = 20;
    private int _spawnCount = 0, _deadCount = 0;
    [SerializeField]
    private float _minDelay = 0.8f, _maxDelay = 1.5f;

    private Animator _animator;
    private readonly int _hashOpen = Animator.StringToHash("open");
    private readonly int _hashClose = Animator.StringToHash("close");

    [Header("사용자 감지 관련")]
    [SerializeField]
    private float _detectRadius = 5f;
    [SerializeField]
    private LayerMask _playerMask;

    [SerializeField]
    private AudioClip _openClip, _closeClip;
    [SerializeField]
    private float _portalOpenDelay = 1f;

    private AudioSource _audioSource;
    private bool _isOpen = false;
    private HealthBar _healthBar;
    
    [Header("포탈 상태 및 이벤트")]
    [SerializeField]
    private bool _sensorActive = false, _passiveActive = false;
    // passiveActive가 true이면 플레이어 접근여부와 상관없이 바로 활성화된다.
    public UnityEvent OnClosePortal = null; //포탈이 닫힐때 발생하는 이벤트
    
    

    private void Awake()
    {
        _animator = transform.Find("VisualSprite").GetComponent<Animator>();
        int playerLayer = LayerMask.NameToLayer("Player");
        _playerMask = 1 << playerLayer;
        _audioSource = GetComponent<AudioSource>();
        _healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    //포탈에게 플레이어를 감지하기 시작하라고 명령
    public void ActivePortalSensor()
    {
        _sensorActive = true;
    }

    private void FixedUpdate()
    {
        if(_isOpen == false && _sensorActive == true)
        {
            if (_passiveActive == true) OpenPortal(); //나중에 만들꺼야
            else
            {
                Collider2D collider = Physics2D.OverlapCircle(transform.position, _detectRadius, _playerMask);
                if (collider != null) OpenPortal();
            }
        }
    }

    private void OpenPortal()
    {
        _isOpen = true;
        _animator.SetTrigger(_hashOpen);
        _audioSource.clip = _openClip;
        _audioSource.Play();

        _healthBar.SetHealth(_count); //적의 갯수만큼 헬스바로 설정함

        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(_portalOpenDelay); //포탈이 열리는 시간동안 대기

        while(_spawnCount < _count) //소환된 갯수가 총 소환할 갯수보다 작은동안 반복
        {
            //여기를 나중에 가중치를 배워서 다시 만들어보세요. 가중치에 따라 나오도록
            int randomIndex = Random.Range(0, _enemyList.Count);

            Vector2 randomOffset = Random.insideUnitCircle;
            randomOffset.y = -Mathf.Abs(randomOffset.y);

            EnemyDataSO spawnEnemyData = _enemyList[randomIndex];

            Vector3 posToSpawn = transform.position;
            Enemy enemy = SpawnEnemy(posToSpawn, spawnEnemyData);
            enemy.SpawnInPortal(transform.position + (Vector3)randomOffset, power: 2f, time: 0.8f);

            UnityAction deadAction = null;
            deadAction = () => {
                _deadCount++;
                _healthBar.SetHealth(_count - _deadCount);
                if (_deadCount == _count)
                {
                    ClosePortal(); //샷따 내려!
                }
                enemy.OnDie.RemoveListener(deadAction);
            };

            enemy.OnDie.AddListener(deadAction);

            float waitTime = Random.Range(_minDelay, _maxDelay);
            _spawnCount++;

            yield return new WaitForSeconds(waitTime); //랜덤한 대기시간 이후 생성
        }
    
    }

    private Enemy SpawnEnemy(Vector3 posToSpawn, EnemyDataSO enemyData)
    {
        Enemy e = PoolManager.Instance.Pop(enemyData.enemyName) as Enemy;
        e.transform.position = posToSpawn;
        return e;
    }

    private void ClosePortal()
    {
        _animator.SetTrigger(_hashClose);
        _audioSource.clip = _closeClip;
        _audioSource.Play();

        _healthBar.gameObject.SetActive(false);
        StartCoroutine(DestroyPortal());
    }

    IEnumerator DestroyPortal()
    {
        yield return new WaitForSeconds(2f);
        
        OnClosePortal?.Invoke();

        gameObject.SetActive(false); //Destroy해버려도 된다.
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}







