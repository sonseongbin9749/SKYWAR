using UnityEngine;
using static Define;

[System.Serializable]
[CreateAssetMenu(menuName = "SO/Item/ResourceData")]
public class ResourceDataSO : ScriptableObject
{
    public float rate; //이 아이템이 드랍될 확률
    public GameObject itemPrefab;

    [field:SerializeField]
    public ResourceTypeEnum ResourceType { get; set; }
    [SerializeField]
    private int minAmount = 1, maxAmount = 5;

    public AudioClip useSound;

    public int GetAmount()
    {
        return Random.Range(minAmount, maxAmount + 1);
    }

    public Color popupTextColor;
}
