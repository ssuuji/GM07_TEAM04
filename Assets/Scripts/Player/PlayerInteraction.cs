using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private bool isInShop; //상점 NPC 범위안인지 확인 여부
    public bool IsInShop => isInShop;
    public void Interact()
    {
        //상점NPC 범위라면
        if (IsInShop)
        {
            Debug.Log("상점 오픈");
            return;
        }

        Debug.Log("상호작용");
    }

    //상점 NPC 범위 상태 변경
    public void SetInShop(bool value)
    {
        isInShop = value;
    }
}