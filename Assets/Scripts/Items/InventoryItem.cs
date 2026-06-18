using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    [SerializeField] private Item itemData;   // 아이템 데이터
    [SerializeField] private int amount;      // 아이템 개수

    // 프로퍼티
    public Item ItemData => itemData;
    public int Amount => amount;
    // 아이템 개별 정보 (강화 수치 등)들은 여기서 관리

    public InventoryItem(Item data, int amount)
    {
        this.itemData = data;
        this.amount = amount;
    }

    /*=============== Method ===============*/

    // 아이템 소지 개수 증가를 위한 메서드
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
    // 아이템 소지 개수 감소를 위한 메서드
    public void SubAmount(int amount)
    {
        this.amount -= amount;
    }
}