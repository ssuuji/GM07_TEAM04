using UnityEngine;

public class SkullManFlip : MonoBehaviour
{
    [SerializeField] private SkullMan skullMan;

    private void Awake()
    {
        skullMan = GetComponentInParent<SkullMan>();
    }


    private void Update()
    {
        FlipSet();
    }

    private void FlipSet()
    {
        transform.localScale = new Vector3(skullMan.Direction ? 1 : -1, 1, 1);
    }
}

