using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotCoolDown : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;

    private Coroutine cooldownCoroutine;

    private void Awake()
    {
        if (cooldownImage != null)
        {
            cooldownImage.gameObject.SetActive(false);
            cooldownImage.fillAmount = 0f;
        }
    }

    /*=============== Method ===============*/

    public void StartCooldown(float duration)
    {
        // 이미 돌고 있는 쿨타임 연출이 있다면 중지
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(CooldownRoutine(duration));
    }
    private IEnumerator CooldownRoutine(float duration)
    {
        if (cooldownImage == null) yield break;

        cooldownImage.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // RPG 정석: 사용 직후 불투명 이미지가 1(꽉 참)이었다가 0(사라짐)으로 줄어드는 방식
            cooldownImage.fillAmount = 1f - (elapsed / duration);

            // 만약 반대로 차오르는 게 좋다면 아래 코드를 쓰세요.
            // cooldownImage.fillAmount = elapsed / duration;

            yield return null; // 다음 프레임까지 대기
        }

        // 쿨타임 종료 세팅
        cooldownImage.fillAmount = 0f;
        cooldownImage.gameObject.SetActive(false);
    }
}