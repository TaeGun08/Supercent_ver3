using UnityEngine;

public interface IMiningTarget
{
    public Transform Transform { get; }
    
    public Cobblestone MineResource();
}