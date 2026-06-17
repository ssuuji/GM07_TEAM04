using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("기본스텟")]
    [SerializeField] private int maxHp = 100; //최대 HP
    [SerializeField] private int maxMP = 100; //최대 MP
    [SerializeField] private int attack = 10; //공격력
    [SerializeField] private int defense = 3; //방어력

    public int MaxHp => maxHp;
    public int MaxMp => maxMP;
    public int Attack => attack;
    public int Defense => defense;

    public void AddMaxHp(int amount)
    {
        maxHp += amount;
    }
    
    public void AddMaxMp(int amount)
    {
        maxMP += amount;
    }

    public void AddAttack(int amount)
    {
        attack += amount;
    }

    public void AddDefense(int amount)
    {
        defense += amount;
    }
}
