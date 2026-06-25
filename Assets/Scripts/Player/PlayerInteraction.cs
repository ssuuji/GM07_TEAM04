using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private bool isInShop;     // 상점 NPC 범위안인지 확인 여부
    public bool IsInShop => isInShop;

    [SerializeField] private bool isInSmith;    // 대장간 NPC 범위인지 확인 여부
    public bool IsInSmith => isInSmith;

    [SerializeField] private bool isInChest;    // 상자 범위 안인지 확인 여부
    private Chest currentChest;                 // 상호작용 가능한 상자

    [SerializeField] private bool isInPortal;   // 포탈 범위 안인지 확인 여부
    private Teleport currentPortal;                // 상호작용 가능한 포탈

    public void Interact()
    {
        //상자 범위 안이라면 상자오픈
        if (isInChest && currentChest != null)
        {
            currentChest.ChestOpen();
            return;
        }

        // 포탈 범위 안이라면 텔레포트
        if (isInPortal && currentPortal != null)
        {
            currentPortal.TeleportPlayer(transform);
            return;
        }

        //상점NPC 범위라면
        if (IsInShop)
        {
            Debug.Log("상점 오픈");
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.ToggleShop();
            }
        }
        else
        {
            if (ShopManager.Instance != null && ShopManager.Instance.IsShopOpend())
            {
                ShopManager.Instance.ToggleShop();
            }
        }
        // 대장간 NPC와 상호작용이 가능한 상태라면
        if (IsInSmith)
        {
            Debug.Log("대장간 오픈");
            if (SmithUI.Instance != null)
            {
                SmithUI.Instance.ToggleSmithUI();
            }
        }
        else
        {
            if (SmithUI.Instance != null && SmithUI.Instance.IsOpened)
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
    // 상자 범위 상태 변경
    public void SetInChest(Chest chest, bool value)
    {
        isInChest = value;

        //Enter : 상자 범위에 들어오면 currentChest 저장
        if (value)
        {
            currentChest = chest;
        }
        //Exit : 현재 등록된 상자 범위를 나가면 currentChest 초기화
        else if (currentChest == chest)
        {
            currentChest = null;
        }
    }
    // 포탈 범위 상태 변경
    public void SetInPortal(Teleport portal, bool value)
    {
        isInPortal = value;

        // Enter : 포탈 범위에 들어오면 현재 포탈 저장
        if (value)
        {
            currentPortal = portal;
        }
        // Exit : 현재 등록된 포탈 범위를 벗어나면 초기화
        else if (currentPortal == portal)
        {
            currentPortal = null;
        }
    }
}