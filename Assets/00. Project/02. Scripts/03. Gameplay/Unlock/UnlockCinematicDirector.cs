using UnityEngine;

public class UnlockCinematicDirector : MonoBehaviour
{
    private void Start()
    {
        // 씬에 존재하는 모든 UnlockZone을 찾아 이벤트를 구독합니다.
        UnlockZone[] zones = FindObjectsOfType<UnlockZone>(true);
        foreach (var zone in zones)
        {
            zone.OnUnlocked += PlayUnlockCinematic;
        }
    }

    private void PlayUnlockCinematic(Transform lookTarget, Transform cameraPoint)
    {
        // [Presentation Layer]: 카메라 연출 실행
        if (CameraDirector.Instance != null && (lookTarget != null || cameraPoint != null))
        {
            CameraDirector.Instance.ShowTarget(lookTarget, cameraPoint);
        }
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지를 위한 구독 해제
        UnlockZone[] zones = FindObjectsOfType<UnlockZone>(true);
        foreach (var zone in zones)
        {
            zone.OnUnlocked -= PlayUnlockCinematic;
        }
    }
}
