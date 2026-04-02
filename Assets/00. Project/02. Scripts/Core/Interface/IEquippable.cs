using UnityEngine;

public interface IEquippable
{
    public void GetMineDelay();
    
    public void Equip(PlayerController player);
    public void UnEquip(PlayerController player);
}
