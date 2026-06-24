using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator chestAnim;
    private bool isOpen;

    //상자 오픈
    public void ChestOpen()
    {
        if (isOpen) return;

        isOpen = true;

        if (chestAnim != null)
        {
            chestAnim.SetTrigger("Open");
        }

        Debug.Log("상자오픈");
    }
}
