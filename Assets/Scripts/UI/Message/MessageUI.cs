using System.Collections;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [Header("메세지 UI")]
    [SerializeField] private Transform messageContainer;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private float messageTime = 2.0f;

    //메세지함수
    public void ShowMessage(string message)
    {
        //메세지프리펩 생성
        GameObject msg = Instantiate(messagePrefab, messageContainer);

        //문구표시
        TextMeshProUGUI Messagetext = msg.GetComponent<TextMeshProUGUI>();
        Messagetext.text = message;

        StartCoroutine(DelMessageCo(msg));
    }

    //메세지 출력 후 삭제
    IEnumerator DelMessageCo(GameObject msg)
    {
        yield return new WaitForSeconds(messageTime);

        Destroy(msg);
    }
}
