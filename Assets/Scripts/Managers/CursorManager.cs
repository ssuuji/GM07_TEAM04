using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [Header("커서")]
    [SerializeField] private Texture2D cursorTexture;      // 커서 이미지
    [SerializeField] private Vector2 hotSpot = Vector2.zero; // 클릭 위치

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetCursor(cursorTexture);
    }

    // 커서 이미지 적용
    public void SetCursor(Texture2D newCursor)
    {
        Cursor.SetCursor(newCursor, hotSpot, CursorMode.Auto);
    }
}
