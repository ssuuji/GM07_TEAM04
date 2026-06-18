using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]

public class CollectableItem : MonoBehaviour
{
    [Header("CollectableItem Data")]
    [SerializeField] protected Item item;   // 아이템 데이터
    [SerializeField] private bool shouldDeleteAfterCollected = true;    // 아이템 획득 시 제거할지
    [SerializeField] protected UnityEvent OnItemCollected;  // 아이템 획득 시 발생할 이벤트

    protected Transform refTransform;

    // 프로퍼티
    public Item Item => item;
    public bool HasCollected { get; protected set; } = false;   // 아이템 수집 여부

    protected virtual void Awake()
    {
        if (refTransform == null)
        {
            refTransform = transform;
        }
    }

    /*=============== Method ===============*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollect(other);
    }
    protected virtual void OnCollect(Collider2D other)
    {
        // 예외 처리
        if (item == null) return;
        if (HasCollected || !other.CompareTag("Player")) return;

        // 매니저에 아이템 정보 전달
        bool isGetItem = InventoryManager.Instance.AddItem(item, 1);
        // 예외 처리
        if (!isGetItem) return;  // 아이템 획득 실패 시 종료

        // 아이템 수집 성공
        OnItemCollected?.Invoke();  // 아이템 수집 이벤트 호출
        HasCollected = true;        // 아이템 수장 상태 변경

        if (shouldDeleteAfterCollected)
        {
            Destroy(gameObject);
        }
        
        // 다이얼로그에 아이템 수집 메세지 전달
    }
}