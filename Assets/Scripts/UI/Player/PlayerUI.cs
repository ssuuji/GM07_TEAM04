using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private PlayerSkill playerSkill;

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

    [Header("레벨 UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expText;

    private void Start()
    {
        InitUI();
    }
    private void Update()
    {
        UpdateStats();
        UpdateSkill();
        UpdateLevel();
        UpdateSkillUnlock();
    }

    private void InitUI()
    {
        hpSlider.maxValue = playerStatus.CurrentMaxHp;
        mpSlider.maxValue = playerStatus.CurrentMaxMp;
        expSlider.maxValue = playerLevel.MaxExp;

        UpdateStats();
    }

    private void UpdateStats()
    {
        hpSlider.value = playerStatus.CurrentHp;
        mpSlider.value = playerStatus.CurrentMp;

        hpText.text = $"{playerStatus.CurrentHp} / {playerStatus.CurrentMaxHp}";
        mpText.text = $"{playerStatus.CurrentMp} / {playerStatus.CurrentMaxMp}";
    }

    private void UpdateSkill()
    {
        UpdateCooldownUI(playerAttack.AreaAttackCooldownRemain, areaAttackCooldownImage, areaAttackCooldownText);
        UpdateCooldownUI(playerAttack.BuffCooldownRemain, buffCooldownImage, buffCooldownText);
        UpdateCooldownUI(playerAttack.InvinCooldownRemain, invinCooldownImage, invinCooldownText);
        UpdateUseBuffUI();
    }
    private void UpdateCooldownUI(float remainTime, Image cooldownImage, TextMeshProUGUI text)
    {
        if (remainTime > 0)
        {
            cooldownImage.gameObject.SetActive(true);
            text.gameObject.SetActive(true);

            text.text = Mathf.CeilToInt(remainTime).ToString(); //소수를 정수로 변환
        }
        else
        {
            cooldownImage.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }

    private void UpdateUseBuffUI()
    {
        float remainTime = playerAttack.BuffDurationRemain;

        if (remainTime > 0)
        {
            useBuffImage.gameObject.SetActive(true);
            useBuffText.gameObject.SetActive(true);

            useBuffText.text = Mathf.CeilToInt(remainTime).ToString();
        }
        else
        {
            useBuffImage.gameObject.SetActive(false);
            useBuffText.gameObject.SetActive(false);
        }
    }

    private void UpdateLevel()
    {
        expSlider.maxValue = playerLevel.MaxExp;
        expSlider.value = playerLevel.CurrentExp;

        levelText.text = $"Lv. {playerLevel.Level}";
        expText.text = $"{playerLevel.CurrentExp} / {playerLevel.MaxExp}";
    }

    private void UpdateSkillUnlock()
    {
        UpdateLockUI(playerSkill.AreaAttackUnlocked, areaAttackLockImage);
        UpdateLockUI(playerSkill.BuffUnlocked, buffLockImage);
        UpdateLockUI(playerSkill.InvinUnlocked, invinLockImage);
    }

    private void UpdateLockUI(bool isUnlocked, Image lockIamage)
    {
        lockIamage.gameObject.SetActive(!isUnlocked);
    }
}
