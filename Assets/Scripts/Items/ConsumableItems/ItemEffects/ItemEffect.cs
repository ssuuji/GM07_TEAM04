using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    // 아이템 설명 추상 프로퍼티
    public virtual string Description => "효과 정보 작성";
    // 소모품의 사용 효과를 작성할 추상 메서드
    public abstract void ExecuteEffect(GameObject target);
}