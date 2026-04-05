using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    [SerializeField] private ItemDataSO resourceData; // 배출할 아이템의 데이터
    public Transform Transform => transform;
    
    private StoneGenerator stoneGenerator;

    public void Init(StoneGenerator generator)
    {
        stoneGenerator = generator;
    }

    public IPickupAble MineResource(bool isFull)
    {
        stoneGenerator?.RegisterRespawn(this);

        gameObject.SetActive(false);
        
        if (isFull) return null;

        Cobblestone newCobble = PoolManager.Instance.Get<Cobblestone>();
        if (newCobble == null) return null;

        newCobble.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        newCobble.gameObject.SetActive(true);

        // [Fix]: 리소스 데이터가 누락되지 않았는지 확인 (없다면 프리팹 기본값 기대)
        if (newCobble.Data == null)
        {
            Debug.LogError($"[Stone] 생성된 Cobblestone에 ItemData가 없습니다! 프리팹 설정을 확인하세요.");
        }

        return newCobble;
    }
}
