using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    // 소모품의 사용 효과를 작성할 추상 메서드
    public abstract void ExecuteEffect(GameObject target);
}