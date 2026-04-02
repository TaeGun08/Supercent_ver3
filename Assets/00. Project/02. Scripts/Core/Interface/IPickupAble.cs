using UnityEngine;

public interface IPickupAble
{
    public Transform Transform { get; }
    
    /// <summary>
    /// 아이템을 풀로 반납하거나 제거할 때 호출
    /// </summary>
    void Release();
}
