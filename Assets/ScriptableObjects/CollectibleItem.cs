using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItem", menuName = "Scriptable Objects/CollectibleItem")]
public class CollectibleItem : ScriptableObject
{
    public string itemId;
    public GameObject item;

    public bool isKey;
}
