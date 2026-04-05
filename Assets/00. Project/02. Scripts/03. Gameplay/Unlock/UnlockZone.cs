using System.Collections;
using UnityEngine;
using TMPro;
using Project.Core.Interfaces;
using UnityEngine.UI;

public class UnlockZone : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private UnlockDataSO unlockData;
    
    [Header("Settings")]
    [SerializeField] private float absorbInterval = 0.1f;
    [SerializeField] private Transform goldTargetPoint;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image costImage;

    private int currentPaidGold;
    private int playerLayer;
    private Coroutine absorbRoutine;
    private IUnlockReward reward;
    private bool isUnlocked;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        reward = GetComponent<IUnlockReward>();
        
        UpdateVisuals();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked || other.gameObject.layer != playerLayer) return;
        
        absorbRoutine ??= StartCoroutine(AbsorbGoldRoutine(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (absorbRoutine == null) return;
        StopCoroutine(absorbRoutine);
        absorbRoutine = null;
    }

    private IEnumerator AbsorbGoldRoutine(GameObject playerObj)
    {
        if (playerObj.TryGetComponent(out Inventory inventory) == false) yield break;
        ItemStacker goldStacker = inventory.GoldStacker;
        WaitForSeconds wait = new WaitForSeconds(absorbInterval);

        while (currentPaidGold < unlockData.RequiredGold)
        {
            if (goldStacker.HasItem)
            {
                IPickupAble gold = goldStacker.PopStack();
                if (gold != null)
                {
                    currentPaidGold++;
                    UpdateVisuals();
                    
                    DOParabolicMove.MoveToStaticPosition(
                        gold.Transform,
                        goldTargetPoint != null ? goldTargetPoint.position : transform.position,
                        height: 2f,
                        duration: 0.3f,
                        onComplete: () => gold.Release()
                    );
                }
            }
            
            if (currentPaidGold >= unlockData.RequiredGold)
            {
                Unlock();
                yield break;
            }

            yield return wait;
        }
    }

    private void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        
        AudioManager.Instance.Play(SoundType.UnlockSuccess);
        reward?.Execute();
        
        if (costText != null) costText.text = "UNLOCKED";
        
        // [Tutorial Hook]: 해금 완료 알림
        TutorialManager.Instance.OnActionPerform(TutorialCondition.UnlockZone);
        
        gameObject.SetActive(false); 
    }

    private void UpdateVisuals()
    {
        if (costText == null || unlockData == null) return;
        int remaining = unlockData.RequiredGold - currentPaidGold;
        costImage.fillAmount = (float)currentPaidGold / unlockData.RequiredGold;
        costText.text = remaining > 0 ? remaining.ToString() : "0";
    }
}
