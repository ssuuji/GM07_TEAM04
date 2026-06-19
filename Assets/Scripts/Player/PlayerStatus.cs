using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("플레이어 기본 스텟")]
    [SerializeField] private PlayerBaseStatSO baseStat;

    [Header("HP/MP")]
    [SerializeField] private int currentHp;
    [SerializeField] private int currentMp;
    [SerializeField] private int currentMaxHp;
    [SerializeField] private int currentMaxMp;

    [Header("현재 공격력/방어력")]
    [SerializeField] private int currentAttack;
    [SerializeField] private int currentDefense;

    public int CurrentMaxHp => currentMaxHp;
    public int CurrentMaxMp => currentMaxMp;
    public int CurrentHp => currentHp;
    public int CurrentMp => currentMp;
    public int CurrentAttack => currentAttack;
    public int CurrentDefense => currentDefense;

    private void Start()
    {
        InitStatus();
    }
    private void InitStatus()
    {
        currentMaxHp = baseStat.MaxHp;
        currentMaxMp = baseStat.MaxMp;
        
        currentHp = currentMaxHp;
        currentMp = currentMaxMp;
        
        currentAttack = baseStat.Attack;
        currentDefense = baseStat.Defense;
    }

    //체력 감소
    public void TakeDamage(int damage)
    {
        int takeDamage = damage - currentDefense;

        if (takeDamage < 1)
        {
            takeDamage = 1;
        }

        currentHp -= takeDamage;

        if (currentHp <= 0)
        {
            currentHp = 0;
        }

        Debug.Log($"플레이어 받은 데미지 {takeDamage} 현재 체력 : {currentHp}");
    }

    //체력 회복
    public void Heal(int amount)
    {
        currentHp += amount;

        if (currentHp > currentMaxHp)
        {
            currentHp = currentMaxHp;
        }
    }

    //마나 회복
    public void RecoverMp(int amount)
    {
        currentMp += amount;

        if (currentMp > currentMaxMp)
        {
            currentMp = currentMaxMp;
        }
    }

    //마나 소모
    public bool UseMp(int amount)
    {
        if (currentMp < amount)
        {
            Debug.Log("마나가 부족합니다.");
            return false;
        }
        else
        {
            currentMp -= amount;
            return true;
        }
    }

    //공격력 증가
    public void AddAttack(int amount)
    {
        currentAttack += amount;
    }
    //공격력 감소
    public void RemoveAttack(int amount)
    {
        currentAttack -= amount;

        if (currentAttack < 0)
        {
            currentAttack = 0;
        }
    }

    //방어력 증가
    public void AddDefense(int amount)
    {
        currentDefense += amount;
    }

    //방어력 감소
    public void RemoveDefense(int amount)
    {
        currentDefense -= amount;

        if (currentDefense < 0)
        {
            currentDefense = 0;
        }
    }

    //최대체력 증가
    public void AddMaxHp(int hp)
    {
        currentMaxHp += hp;
    }

    //최대마나 증가
    public void AddMaxMp(int mp)
    {
        currentMaxMp += mp;
    }
}
