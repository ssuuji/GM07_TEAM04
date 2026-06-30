using UnityEngine;

public class SkullManAttackPoint : MonoBehaviour
{
    [SerializeField] private SkullMan skullMan;

    private void Awake()
    {
        skullMan = GetComponentInParent<SkullMan>();
    }

    private void Update()
    {
        SetPos();
    }

    private void SetPos()
    { 
        if (skullMan.Direction)
        {
            transform.localPosition = new Vector3(0.6f, 0.8f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-0.6f, 0.8f, 0);
        }
    }

}
