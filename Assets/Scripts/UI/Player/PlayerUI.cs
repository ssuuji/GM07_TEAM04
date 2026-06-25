using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private GameObject player; //플레이어
     private PlayerStatus playerStatus;
     private PlayerAttack playerAttack;
     private PlayerLevel playerLevel;
     private PlayerSkill playerSkill;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("MP UI")]
    [SerializeField] private Slider mpSlider;
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("범위공격 UI")]
    [SerializeField] private Image areaAttackCooldownImage;
    [SerializeField] private TextMeshProUGUI areaAttackCooldownText;
    [SerializeField] private Image areaAttackLockImage;

    [Header("공격버프 UI")]
    [SerializeField] private Image buffCooldownImage;
    [SerializeField] private TextMeshProUGUI buffCooldownText;
    [SerializeField] private Image buffLockImage;
    [SerializeField] private Image useBuffImage;
    [SerializeField] private TextMeshProUGUI useBuffText;

    [Header("무적기 UI")]
    [SerializeField] private Image invinCooldownImage;
    [SerializeField] private TextMeshProUGUI invinCooldownText;
    [SerializeField] private Image invinLockImage;
    [SerializeField] private Image useInvinImage;
    [SerializeField] private TextMeshProUGUI useInvinText;

    [Header("레벨 UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expText;

    private void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        playerAttack = player.GetComponent<PlayerAttack>();
        playerLevel = player.GetComponent<PlayerLevel>();
        playerSkill = player.GetComponent<PlayerSkill>();

        InitUI(); //UI 초기 설정
    }
    private void Update()
    {
        UpdateStats();       // HP/MP UI갱신
        UpdateSkill();       // 스킬쿨타임 UI갱신
        UpdateLevel();       // 레벨/경험치 UI갱신
        UpdateSkillUnlock(); // 스킬잠금 UI갱신
    }

    //UI 초기 설정
    private void InitUI()
    {
        hpSlider.maxValue = playerStatus.CurrentMaxHp; //HP 슬라이더 최대값 설정
        mpSlider.maxValue = playerStatus.CurrentMaxMp; //MP 슬라이더 최대값 설정
        expSlider.maxValue = playerLevel.MaxExp;       //경험치 슬라이더 최대값 설정

        UpdateStats(); // HP/MP UI갱신
    }

    // HP/MP UI갱신
    private void UpdateStats()
    {
        hpSlider.value = playerStatus.CurrentHp; //현재 HP반영
        mpSlider.value = playerStatus.CurrentMp; //현재 MP반영

        hpText.text = $"{playerStatus.CurrentHp} / {playerStatus.CurrentMaxHp}";
        mpText.text = $"{playerStatus.CurrentMp} / {playerStatus.CurrentMaxMp}";
    }

    // 스킬쿨타임 UI갱신
    private void UpdateSkill()
    {
        //각 스킬의 남은 쿨타임 UI갱신
        UpdateCooldownUI(playerAttack.AreaAttackCooldownRemain, areaAttackCooldownImage, areaAttackCooldownText);
        UpdateCooldownUI(playerAttack.BuffCooldownRemain, buffCooldownImage, buffCooldownText);
        UpdateCooldownUI(playerAttack.InvinCooldownRemain, invinCooldownImage, invinCooldownText);
        
        //버프 사용중 UI갱신 (공격버프, 무적기)
        UpdateUseSkillUI();
    }
    private void UpdateCooldownUI(float remainTime, Image cooldownImage, TextMeshProUGUI text)
    {
        //쿨타임이 남아있으면 이미지와 남은시간텍스트 활성화
        if (remainTime > 0)
        {
            cooldownImage.gameObject.SetActive(true);
            text.gameObject.SetActive(true);

            text.text = Mathf.CeilToInt(remainTime).ToString(); //남은시간 소수를 정수로 변환
        }
        //쿨타임 끝나면 이미지, 텍스트 비활성화
        else
        {
            cooldownImage.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }

    private void UpdateUseSkillUI()
    {
        float buffRemainTime = playerAttack.BuffDurationRemain;   //공격버프 남은 지속시간
        float invinRemainTime = playerAttack.InvinDurationRemain; //무적기 남은 지속시간

        //공격버프가 사용중이면 상단에 버프 UI표시
        if (buffRemainTime > 0)
        {
            useBuffImage.gameObject.SetActive(true);
            useBuffText.gameObject.SetActive(true);

            useBuffText.text = Mathf.CeilToInt(buffRemainTime).ToString();
        }
        //공격버프 끝나면 버프UI 비활성화
        else
        {
            useBuffImage.gameObject.SetActive(false);
            useBuffText.gameObject.SetActive(false);
        }


        //무적기가 사용중이면 상단에 무적기 UI표시
        if (invinRemainTime > 0)
        {
            useInvinImage.gameObject.SetActive(true);
            useInvinText.gameObject.SetActive(true);

            useInvinText.text = Mathf.CeilToInt(invinRemainTime).ToString();
        }
        //무적기 끝나면 무적기UI 비활성화
        else
        {
            useInvinImage.gameObject.SetActive(false);
            useInvinText.gameObject.SetActive(false);
        }
    }

    private void UpdateLevel()
    {
        expSlider.maxValue = playerLevel.MaxExp;   //현재 레벨의 Max경험치 
        expSlider.value = playerLevel.CurrentExp;  //현재 경험치

        levelText.text = $"Lv. {playerLevel.Level}";                       //레벨 텍스트 표시
        expText.text = $"{playerLevel.CurrentExp} / {playerLevel.MaxExp}"; //경험치 슬라이더바 위에 텍스트 표시
    }

    private void UpdateSkillUnlock()
    {
        //스킬 해금 상태에 따라 잠금이미지(현재는 검정이미지) 표시 여부 갱신
        UpdateLockUI(playerSkill.AreaAttackUnlocked, areaAttackLockImage);
        UpdateLockUI(playerSkill.BuffUnlocked, buffLockImage);
        UpdateLockUI(playerSkill.InvinUnlocked, invinLockImage);
    }

    private void UpdateLockUI(bool isUnlocked, Image lockIamage)
    {
        //스킬 해금 되면 잠금이미지 숨기고 , 잠금상태면 표시하기
        lockIamage.gameObject.SetActive(!isUnlocked);
    }
}
