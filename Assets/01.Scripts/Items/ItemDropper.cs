using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private ItemDropTableSO _dropTable;
    private float[] _itemWeights;

    [SerializeField]
    private bool _dropEffect = false; //점핑이펙트

    [SerializeField]
    private float _dropPower = 2f;
    [SerializeField]
    [Range(0, 1f)]
    private float _dropChance; //이 몹? 의 아이템의 드랍확률

    //3단 
    //1단 : 아이템을 떨굴 것인가?
    //2단 : 어떤 아이템을 떨굴것인가? (단 여기서 가중치를 적용해서 가중치에 따라 드롭

    private void Start()
    {
        _itemWeights = _dropTable.dropList.Select(item => item.rate).ToArray();
    }

    public void DropItem()
    {
        float dropVariable = Random.value; //0 ~1;
        if(dropVariable < _dropChance) //드랍율에 걸렸다면 아이템 드랍
        {
            int idx = GetRandomWeightedIndex();
            Resource resource = PoolManager.Instance.Pop(_dropTable.dropList[idx].itemPrefab.name) as Resource;

            resource.transform.position = transform.position;

            Action destroyAction = null;
            destroyAction = () =>
            {
                resource.DestroyResource();
                GameManager.Instance.OnClearAllDropItems -= destroyAction;
            };
            GameManager.Instance.OnClearAllDropItems += destroyAction;

            if(_dropEffect)
            {
                Vector3 offset = Random.insideUnitCircle;

                resource.transform.DOJump(transform.position + offset, _dropPower, 1, 0.3f);
            }
        }
        // 아니면 아무것도 안뱉음.
    }

    private int GetRandomWeightedIndex()
    {
        float sum = 0f;
        for(int i = 0; i < _itemWeights.Length; i++)
        {
            sum += _itemWeights[i]; //이러면 모든 아이템의 드랍확률이 합산된다.
        }

        float randomValue = Random.Range(0, sum);
        float tempSum = 0;

        for(int i = 0; i < _itemWeights.Length; i++)
        {
            if(randomValue >= tempSum && randomValue < tempSum + _itemWeights[i])
            {
                return i;
            }else
            {
                tempSum += _itemWeights[i];
            }
        }

        return 0;
    }
}
