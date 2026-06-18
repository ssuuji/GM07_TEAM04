using System;
using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
    // 골드 값이 바뀔 때 호출
    public event Action<int> OnGoldChanged;     // 골드 변화 이벤트

    [Header("Gold Info")]
    [SerializeField] private int currentGold;   // 소지 골드량

    // 프로퍼티
    public int CurrentGold => currentGold;

    /*=============== Method ===============*/
    
    // 골드 증가(획득) 메서드
    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
        Debug.Log($"{amount} 골드 획득 ! | 소지 골드 : {currentGold}");
    }
    // 골드 감소(소모) 메서드
    public bool SpendGold(int amount)
    {
        // 예외 처리
        if (currentGold < amount)
        {
            Debug.Log("소지 골드가 부족합니다.");
            return false;
        }
        currentGold -= amount;
        OnGoldChanged?.Invoke(currentGold);
        return true;
    }
}