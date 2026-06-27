using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private GameObject player;

    [Header("스탯창")]
    [SerializeField] private GameObject statusPanel;

    [Header("스탯")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;

    private PlayerStatus playerStatus;
    private PlayerLevel playerLevel;
    private bool isOpen = false; //스탯창이 현재 열려있는지 확인 여부
    

    private void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        playerLevel = player.GetComponent<PlayerLevel>();
        statusPanel.SetActive(false); //스탯창 우선 숨김처리
    }

    private void Update()
    {
        //스탯창 열기/닫기 : Tab키
        if (InputManager.IsStatus)
        {
            OpenStatusUI();
        }

        //스탯창이 열려있는 동안 정보 업데이트
        if (isOpen)
        {
            UpdateStatusUI();
        }
    }

    //스탯창 열기/닫기 : Tab키
    private void OpenStatusUI()
    {
        isOpen = !isOpen;

        statusPanel.SetActive(isOpen);

        //스탯창 열 때 바로 정보 업데이트
        if (isOpen)
        {
            UpdateStatusUI();
        }
    }

    //스탯창 닫기 클릭버튼
    public void CloseStatusUI()
    {
        isOpen = false;
        statusPanel.SetActive(false);
    }

    private void UpdateStatusUI()
    {
        levelText.text = $"Lv. {playerLevel.Level}";
        hpText.text = $"{playerStatus.CurrentHp} / {playerStatus.CurrentMaxHp}";
        mpText.text = $"{playerStatus.CurrentMp} / {playerStatus.CurrentMaxMp}";
        atkText.text = $"{playerStatus.CurrentAttack}";
        defText.text = $"{playerStatus.CurrentDefense}";
    }
}
