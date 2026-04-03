using UnityEngine;

public enum ItemType { Stone, Gold, Potion }

public interface IPickupAble
{
    public Transform Transform { get; }
    public ItemType Type { get; }
    
    /// <summary>
    /// 아이템을 풀로 반납하거나 제거할 때 호출
    /// </summary>
    void Release();
}
