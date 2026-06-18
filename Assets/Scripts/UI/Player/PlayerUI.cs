using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("플레이어 상태")]
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerAttack playerAttack;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("MP UI")]
    [SerializeField] private Slider mpSlider;
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("범위공격 UI")]
    [SerializeField] private Image areaAttackCooldownImage;
    [SerializeField] private TextMeshProUGUI areaAttackCooldownText;

    [Header("공격버프 UI")]
    [SerializeField] private Image buffCooldownImage;
    [SerializeField] private TextMeshProUGUI buffCooldownText;
    [SerializeField] private Image useBuffImage;
    [SerializeField] private TextMeshProUGUI useBuffText;

    [Header("무적기 UI")]
    [SerializeField] private Image invinCooldownImage;
    [SerializeField] private TextMeshProUGUI invinCooldownText;

    private void Start()
    {
        InitUI();
    }
    private void Update()
    {
        UpdateStats();
        UpdateSkill();
    }

    private void InitUI()
    {
        hpSlider.maxValue = playerStatus.MaxHp;
        mpSlider.maxValue = playerStatus.MaxMp;

        UpdateStats();
    }

    private void UpdateStats()
    {
        hpSlider.value = playerStatus.CurrentHp;
        mpSlider.value = playerStatus.CurrentMp;

        hpText.text = $"{playerStatus.CurrentHp} / {playerStatus.MaxHp}";
        mpText.text = $"{playerStatus.CurrentMp} / {playerStatus.MaxMp}";
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

}
