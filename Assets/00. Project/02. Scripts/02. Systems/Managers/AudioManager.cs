using UnityEngine;
using System.Collections.Generic;

public class AudioManager : SingletonBase<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundDataSO soundData;

    private readonly Dictionary<SoundType, AudioClip> clipCache = new();

    protected override void Awake()
    {
        base.Awake();
        
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        Debug.Assert(soundData != null, "[AudioManager] SoundDataSO가 할당되지 않았습니다.");

        InitializeCache();
    }

    private void InitializeCache()
    {
        if (soundData == null) return;
        foreach (var mapping in soundData.soundMappings)
        {
            if (mapping.clip != null && !clipCache.ContainsKey(mapping.type))
                clipCache.Add(mapping.type, mapping.clip);
        }
    }

    public void Play(SoundType type)
    {
        if (type == SoundType.None) return;

        if (clipCache.TryGetValue(type, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] {type}에 해당하는 클립이 캐시에 없습니다.");
        }
    }
}
