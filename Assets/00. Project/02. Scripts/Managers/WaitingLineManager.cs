using System.Collections.Generic;
using UnityEngine;

public class WaitingLineManager : SingletonBase<WaitingLineManager>
{
    [Header("Line Settings")]
    [SerializeField] private List<Transform> linePoints;
    
    private List<Npc> waitingNPCs = new();

    public bool CanJoin => waitingNPCs.Count < linePoints.Count;
    
    public void JoinLine(Npc npc)
    {
        if (!CanJoin) return;

        waitingNPCs.Add(npc);
        int myIndex = waitingNPCs.Count - 1;
        
        npc.MoveTo(linePoints[myIndex].position);
    }
    
    public void OnFrontPersonLeft()
    {
        if (waitingNPCs.Count == 0) return;

        waitingNPCs.RemoveAt(0);
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        for (int i = 0; i < waitingNPCs.Count; i++)
        {
            waitingNPCs[i].MoveTo(linePoints[i].position);
        }
    }

    public Npc GetFrontNpc() => waitingNPCs.Count > 0 ? waitingNPCs[0] : null;
}