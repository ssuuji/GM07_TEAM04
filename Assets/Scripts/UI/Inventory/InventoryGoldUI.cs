using TMPro;
using UnityEngine;

public class InventoryGoldUI : MonoBehaviour
{
    [Header("Gold Text UI")]
    [SerializeField] private TextMeshProUGUI goldText;

    private GoldManager goldManager;

    private void OnEnable()
    {
        goldManager = GoldManager.Instance;
        if (goldManager == null) return;

        goldManager.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(goldManager.CurrentGold);
    }

    private void OnDisable()
    {
        if (goldManager == null) return;

        goldManager.OnGoldChanged -= UpdateGoldUI;
    }

    /*=============== Method ===============*/

    private void UpdateGoldUI(int amount)
    {
        goldText.text = amount.ToString("N0");
    }
}