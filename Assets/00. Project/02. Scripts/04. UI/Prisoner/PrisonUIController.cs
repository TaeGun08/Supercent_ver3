using TMPro;
using UnityEngine;

namespace Project.UI
{
    /// <summary>
    /// 감옥의 전체 수감 인원 상태 UI를 전담하는 컨트롤러
    /// </summary>
    public class PrisonUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text prisonerCountText;

        private void Start()
        {
            // 초기 상태 갱신 (실제 매니저 데이터와 동기화)
            if (PrisonManager.Instance != null)
            {
                UpdateUI(PrisonManager.Instance.CurrentCount, PrisonManager.Instance.MaxCapacity);
                
                // 이벤트 구독
                PrisonManager.Instance.OnPrisonerCountChanged += UpdateUI;
            }
        }

        private void UpdateUI(int current, int max)
        {
            if (prisonerCountText != null)
            {
                prisonerCountText.text = $"{current} / {max}";
            }
        }

        private void OnDestroy()
        {
            if (PrisonManager.Instance != null)
            {
                PrisonManager.Instance.OnPrisonerCountChanged -= UpdateUI;
            }
        }
    }
}
