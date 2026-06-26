using Demo_Project;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerLevel : MonoBehaviour
{
    [Header("레벨 설정")]
    [SerializeField] private int level = 1;    //현재 레벨
    [SerializeField] private int currentExp;   //현재 경험치
    [SerializeField] private int maxExp = 100; //레벨업에 필요한 경험치

    [Header("레벨업 보상")]
    [SerializeField] private int levelUpHp = 10; //레벨업 시 추가 HP
    [SerializeField] private int levelUpMp = 5;  //레벨업 시 추가 MP

    [Header("레벨업 사운드")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip levelUpSound;

    private PlayerStatus playerStatus;
    private PlayerSkill playerSkill;
    private PlayerUI playerUI;
    private MessageUI messageUI;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int MaxExp => maxExp;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerSkill = GetComponent<PlayerSkill>();
        playerUI = FindFirstObjectByType<PlayerUI>();
        audioSource = GetComponent<AudioSource>();
        messageUI = FindFirstObjectByType<MessageUI>();
    }

    //경험치 획득
    public void AddExp(int exp)
    {
        //exp 만큼 현재 경험치 증가
        currentExp += exp;

        

        //현재 경험치가 필요 경험치 이상이면 레벨업
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp; //레벨업에 사용된 경험치 차감
            LevelUp();            //레벨업
        }
    }

    //레벨업
    private void LevelUp()
    {
        level++; //레벨 증가

        //레벨업 UI
        playerUI.ShowLevelUp(level);

        //레벨업 사운드
        audioSource.PlayOneShot(levelUpSound);

        //레벨업 보상 (HP 증가 / MP 증가) + 레벨업시 HP/MP 전부 회복
        playerStatus.AddMaxHp(levelUpHp);
        playerStatus.AddMaxMp(levelUpMp);
        playerStatus.Heal(playerStatus.CurrentMaxHp);
        playerStatus.RecoverMp(playerStatus.CurrentMaxMp);

        //경험치통 증가
        maxExp += 50;

        //스킬해금확인
        CheckSkillUnlock();
    }

    private void CheckSkillUnlock()
    {
        if (level == 2)
        {
            playerSkill.UnlockAreaAttack(); //2레벨 : 범위공격 해금
            messageUI.ShowMessage($"Lv. 2 달성!\n" +
                                  $"범위 공격[X] 스킬 해금!\n" +
                                  $"최대 HP +{levelUpHp} / 최대 MP +{levelUpMp}");
        }

        if (level == 3)
        {
            playerSkill.UnlockBuff(); //3레벨 : 공격버프 해금
            messageUI.ShowMessage($"Lv. 3 달성!\n" +
                                  $"공격버프[C] 스킬 해금!\n" +
                                  $"최대 HP +{levelUpHp} / 최대 MP +{levelUpMp}");
        }

        if (level == 4)
        {
            playerSkill.UnlockInvin(); //4레벨 : 무적기 해금
            messageUI.ShowMessage($"Lv. 4 달성!\n" +
                                  $"무적기[V] 스킬 해금!\n" +
                                  $"최대 HP +{levelUpHp} / 최대 MP +{levelUpMp}");
        }

    }
}
