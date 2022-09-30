using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("�� ����")]
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

    [Header("����� ���� ����")]
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
    
    [Header("��Ż ���� �� �̺�Ʈ")]
    [SerializeField]
    private bool _sensorActive = false, _passiveActive = false;
    // passiveActive�� true�̸� �÷��̾� ���ٿ��ο� ������� �ٷ� Ȱ��ȭ�ȴ�.
    public UnityEvent OnClosePortal = null; //��Ż�� ������ �߻��ϴ� �̺�Ʈ
    
    

    private void Awake()
    {
        _animator = transform.Find("VisualSprite").GetComponent<Animator>();
        int playerLayer = LayerMask.NameToLayer("Player");
        _playerMask = 1 << playerLayer;
        _audioSource = GetComponent<AudioSource>();
        _healthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    //��Ż���� �÷��̾ �����ϱ� �����϶�� ���
    public void ActivePortalSensor()
    {
        _sensorActive = true;
    }

    private void FixedUpdate()
    {
        if(_isOpen == false && _sensorActive == true)
        {
            if (_passiveActive == true) OpenPortal(); //���߿� ���鲨��
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

        _healthBar.SetHealth(_count); //���� ������ŭ �ｺ�ٷ� ������

        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(_portalOpenDelay); //��Ż�� ������ �ð����� ���

        while(_spawnCount < _count) //��ȯ�� ������ �� ��ȯ�� �������� �������� �ݺ�
        {
            //���⸦ ���߿� ����ġ�� ����� �ٽ� ��������. ����ġ�� ���� ��������
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
                    ClosePortal(); //���� ����!
                }
                enemy.OnDie.RemoveListener(deadAction);
            };

            enemy.OnDie.AddListener(deadAction);

            float waitTime = Random.Range(_minDelay, _maxDelay);
            _spawnCount++;

            yield return new WaitForSeconds(waitTime); //������ ���ð� ���� ����
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

        gameObject.SetActive(false); //Destroy�ع����� �ȴ�.
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







