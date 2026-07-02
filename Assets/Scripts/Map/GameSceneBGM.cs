using UnityEngine;

public class GameSceneBGM : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
