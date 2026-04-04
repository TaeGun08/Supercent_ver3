using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonBase<EffectManager>
{
    [System.Serializable]
    public struct EffectData
    {
        public string key;
        public ParticleSystem prefab;
        public int initialCount;
    }

    [Header("Effect Settings")]
    [SerializeField] private List<EffectData> effectList = new();

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var effect in effectList)
        {
            if (effect.prefab == null) continue;
            PoolManager.Instance.CreatePool(effect.prefab, effect.key, effect.initialCount);
        }
    }

    /// <summary>
    /// 지정된 위치에 이펙트를 재생하고 완료 후 자동으로 풀에 반납합니다.
    /// </summary>
    public void PlayEffect(string key, Vector3 position, Quaternion rotation = default, Transform parent = null)
    {
        var ps = PoolManager.Instance.Get<ParticleSystem>(key);
        
        // [Fail-Fast] 등록되지 않은 키나 프리팹 누락 시 즉시 보고
        Debug.Assert(ps != null, $"[EffectManager] '{key}' 에 해당하는 이펙트 풀을 찾을 수 없거나 파티클 시스템이 없습니다.");
        if (ps == null) return;

        Transform tf = ps.transform;
        if (parent != null) tf.SetParent(parent);
        
        tf.position = position;
        tf.rotation = rotation == default ? Quaternion.identity : rotation;

        ps.Play();
        
        // 재생 완료 후 자동 반납 (재사용을 위해 코루틴 사용 - StopAction은 Callback 기반 시 이벤트 처리가 복잡할 수 있음)
        StartCoroutine(ReturnEffectToPool(key, ps));
    }

    private IEnumerator ReturnEffectToPool(string key, ParticleSystem ps)
    {
        // 파티클 시스템의 지속 시간만큼 대기 (가장 긴 파티클 기준)
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        
        // [Fail-Fast] 반납 시점의 객체 유효성 검증
        if (ps != null)
        {
            ps.Stop();
            PoolManager.Instance.Return(ps, key);
        }
    }
}
