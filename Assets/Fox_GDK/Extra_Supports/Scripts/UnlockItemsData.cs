using UnityEngine;

[CreateAssetMenu(menuName = "Luggage_Unblock_Frenzy/Unlock Item", fileName = "Item Name")]
public class UnlockItemsData : ScriptableObject
{
    public Unlock_Item_Type type;
    public Sprite sprite;
    public bool unlocked;
    public int unlockedLevelNumber;
    public int startingLevelNumber;
}
