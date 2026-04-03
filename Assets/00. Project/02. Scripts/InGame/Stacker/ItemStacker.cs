using System.Collections.Generic;
using UnityEngine;

public abstract class ItemStacker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected int maxStackCount = 10;
    [SerializeField] protected float itemHeight = 0.5f;
    
    protected Stack<IPickupAble> stackedItems = new();
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    public int CurrentCount => stackedItems.Count;

    /// <summary>
    /// 다음 아이템이 위치할 로컬 좌표 반환
    /// </summary>
    public Vector3 GetNextLocalPosition()
    {
        return new Vector3(0, stackedItems.Count * itemHeight, 0);
    }

    /// <summary>
    /// 논리적 스택 추가 (데이터 삽입 및 위치 고정)
    /// </summary>
    public virtual void PushStack(IPickupAble pickupAble)
    {
        Debug.Assert(!IsFull, "[ItemStacker] Max Stack Count를 초과하여 Push를 시도했습니다.");
        if (pickupAble == null) return;

        stackedItems.Push(pickupAble);
        
        // 데이터 삽입 시점에 즉시 부모 설정 및 위치 고정
        pickupAble.Transform.SetParent(transform);
        pickupAble.Transform.SetLocalPositionAndRotation(GetNextLocalPosition(), Quaternion.identity);
    }

    /// <summary>
    /// 논리적 스택 추출 (데이터 제거)
    /// </summary>
    public virtual IPickupAble PopStack()
    {
        Debug.Assert(HasItem, "[ItemStacker] 빈 스택에서 Pop을 시도했습니다.");
        
        IPickupAble pickupAble = stackedItems.Pop();
        
        // 추출된 아이템은 부모 관계 해제
        pickupAble.Transform.SetParent(null);
        
        return pickupAble;
    }
}