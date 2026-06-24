using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private bool isInShop;     // 상점 NPC 범위안인지 확인 여부
    public bool IsInShop => isInShop;

    [SerializeField] private bool isInSmith;    // 대장간 NPC 범위인지 확인 여부
    public bool IsInSmith => isInSmith;

    public void Interact()
    {
        //상점NPC 범위라면
        if (IsInShop)
        {
            Debug.Log("상점 오픈");
            if (ShopManager.Instance == null) return;
            ShopManager.Instance.ToggleShop();
        }
        else
        {
            if (ShopManager.Instance == null) return;
            if (ShopManager.Instance.IsShopOpend())
            {
                ShopManager.Instance.ToggleShop();
            }
        }
        // 대장간 NPC와 상호작용이 가능한 상태라면
        if (IsInSmith)
        {
            Debug.Log("대장간 오픈");
            if (SmithUI.Instance == null) return;
            SmithUI.Instance.ToggleSmithUI();
        }
        else
        {
            if (SmithUI.Instance == null) return;
            if (SmithUI.Instance.IsOpened)
            {
                SmithUI.Instance.ToggleSmithUI();
            }
        }

        Debug.Log("상호작용");
        return;
    }

    // 상점 NPC 범위 상태 변경
    public void SetInShop(bool value)
    {
        isInShop = value;
    }
    // 대장간 NPC 상호작용 가능 여부 설정
    public void SetInSmith(bool value)
    {
        isInSmith = value;
    }
}