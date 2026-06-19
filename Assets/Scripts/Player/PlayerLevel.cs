using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("레벨 설정")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int maxExp = 100;

    [Header("레벨업 보상")]
    [SerializeField] private int levelUpHp = 10;
    [SerializeField] private int levelUpMp = 5;

    private PlayerStatus playerStatus;
    private PlayerSkill playerSkill;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int MaxExp => maxExp;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerSkill = GetComponent<PlayerSkill>();
    }

    public void AddExp(int exp)
    {
        currentExp += exp;

        Debug.Log($"경험치 획득 +{exp} | 현재 경험치 {currentExp}/{maxExp}");

        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;

        Debug.Log($"레벨업! 현재 레벨 {level}");

        playerStatus.AddMaxHp(levelUpHp);
        playerStatus.AddMaxMp(levelUpMp);
        maxExp += 50;

        CheckSkillUnlock();
    }

    private void CheckSkillUnlock()
    {
        if (level == 2)
        {
            playerSkill.UnlockAreaAttack();
        }

        if (level == 3)
        {
            playerSkill.UnlockBuff();
        }

        if (level == 4)
        {
            playerSkill.UnlockInvin();
        }

    }
}
