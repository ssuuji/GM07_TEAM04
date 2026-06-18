using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    private int maxHp;

    public void Initialize(int maxHealth)
    {
        maxHp = maxHealth;

        hpFillImage.fillAmount = 1.0f;
    }

    public void SetHP(int currentHp)
    {
        hpFillImage.fillAmount = (float) currentHp / maxHp;
    }

}
