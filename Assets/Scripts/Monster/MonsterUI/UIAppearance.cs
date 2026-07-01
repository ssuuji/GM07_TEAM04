using UnityEngine;

public class UIAppearance : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    

    private void Awake()
    {
        ui.SetActive(false);
    }

    public void Appear()
    {
        ui.SetActive(true);

    }
}
