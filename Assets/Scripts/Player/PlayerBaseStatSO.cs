using UnityEngine;

[CreateAssetMenu(fileName ="PlayerBaseStat", menuName = "ScriptableObject/Player/Base Stat")]
public class PlayerBaseStatSO : ScriptableObject
{
    [Header("HP/MP")]
    [SerializeField] private int maxHp = 100; //최대 HP
    [SerializeField] private int maxMP = 100; //최대 MP

    [Header("전투")]
    [SerializeField] private int attack = 10; //공격력
    [SerializeField] private int defense = 3; //방어력

    public int MaxHp => maxHp;
    public int MaxMp => maxMP;
    public int Attack => attack;
    public int Defense => defense;
}
