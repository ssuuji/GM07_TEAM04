using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float PlayTime { get; private set; }
    public int KillCount { get; private set; }

    private bool isPlaying;

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        PlayTime += Time.deltaTime;
    }

    // 게임 시작
    public void StartTimer()
    {
        isPlaying = true;
    }

    // 보스 처치 후 
    public void StopTimer()
    {
        isPlaying = false;
    }

    // 몬스터 수 추가
    public void AddKillCount()
    {
        KillCount++;
    }

    //초기화
    public void ResetStats()
    {
        PlayTime = 0f;
        KillCount = 0;
        isPlaying = false;
    }

    //플레이시간
    public string GetPlayTimeText()
    {
        int totalSeconds = Mathf.FloorToInt(PlayTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}
