using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    public Transform Transform => transform;
    
    // [Pooling]: 타입 기반 풀링이므로 프리팹 참조는 생성 시에만 필요하지만, 
    // 기존 데이터 호환성을 위해 필드는 유지하되 로직은 PoolManager를 사용합니다.
    
    private StoneGenerator stoneGenerator;

    public void Init(StoneGenerator generator)
    {
        stoneGenerator = generator;
    }

    public IPickupAble MineResource(bool isFull)
    {
        // 리스폰 큐에 등록
        stoneGenerator?.RegisterRespawn(this);

        // 현재 돌은 비활성화 (풀에서 제거하는 것이 아니라 맵에서만 일시적으로 사라짐)
        gameObject.SetActive(false);
        
        if (isFull) return null;

        // [Pooling 적용]: Instantiate 대신 Pool에서 Cobblestone 획득
        Cobblestone newCobble = PoolManager.Instance.Get<Cobblestone>();
        Debug.Assert(newCobble != null, "Stone: Failed to get Cobblestone from Pool!");

        // 위치 및 활성화 상태 초기화
        newCobble.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        newCobble.gameObject.SetActive(true);

        return newCobble;
    }
}
