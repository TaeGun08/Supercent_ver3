using System.Collections.Generic;
using UnityEngine;

public class WaitingLineManager : SingletonBase<WaitingLineManager>
{
    [Header("Line Settings")]
    [SerializeField] private List<Transform> linePoints;
    
    private readonly List<Npc> waitingNPCs = new();

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
        // 리스트 정화 (Destroyed objects 등 제거)
        waitingNPCs.RemoveAll(npc => npc == null);

        for (int i = 0; i < waitingNPCs.Count; i++)
        {
            Vector3 targetPos = linePoints[i].position;
            
            // [Optimization]: 이미 목표 지점 근처에 있다면 이동 명령 생략
            if (Vector3.Distance(waitingNPCs[i].transform.position, targetPos) < 0.1f) continue;
            
            waitingNPCs[i].MoveTo(targetPos);
        }
    }

    public Npc GetFrontNpc()
    {
        // 유효한 맨 앞 NPC 반환
        while (waitingNPCs.Count > 0 && waitingNPCs[0] == null)
        {
            waitingNPCs.RemoveAt(0);
        }
        return waitingNPCs.Count > 0 ? waitingNPCs[0] : null;
    }

    public Vector3 GetPoint(int index)
    {
        if (index < 0 || index >= linePoints.Count) return Vector3.zero;
        return linePoints[index].position;
    }
}