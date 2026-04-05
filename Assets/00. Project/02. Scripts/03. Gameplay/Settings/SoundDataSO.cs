using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData")]
public class SoundDataSO : ScriptableObject
{
    [System.Serializable]
    public struct SoundMapping
    {
        public SoundType type;
        public AudioClip clip;
    }

    public List<SoundMapping> soundMappings;

    public AudioClip GetClip(SoundType type)
    {
        var mapping = soundMappings.Find(m => m.type == type);
        return mapping.clip;
    }
}
