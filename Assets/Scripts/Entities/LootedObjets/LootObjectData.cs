using UnityEngine;

[CreateAssetMenu(fileName = "Loots", menuName = "Loot")]
public class LootObjectData : ScriptableObject
{
    public int itemId;
    public string objectName;
    public float minAmount;
    public float maxAmount;
    [Range(0.001f, 1)] public float dropRate;
    public GameObject prefab;
    public Sprite picture;
}
