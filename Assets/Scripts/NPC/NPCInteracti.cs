using UnityEngine;

public class NPCInteracti : MonoBehaviour
{
    [Header("NPC Setting")]
    [SerializeField] private NPCType npcType;

    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        if (playerInteraction == null)
        {
            playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        }
    }

    /*=============== Method ===============*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (npcType == NPCType.Shop)
            {
                playerInteraction.SetInShop(true);
            }
            else if (npcType == NPCType.Smith)
            {
                playerInteraction.SetInSmith(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (npcType == NPCType.Shop)
            {
                playerInteraction.SetInShop(false);
                playerInteraction.Interact();
            }
            else if (npcType == NPCType.Smith)
            {
                playerInteraction.SetInSmith(false);
                playerInteraction.Interact();
            }
        }
    }
}