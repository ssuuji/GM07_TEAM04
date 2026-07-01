using UnityEngine;

public class PoolPreloader : MonoBehaviour
{


    [Header("프리팹")]
    [SerializeField] private Monster monsterPrefab;
    //[SerializeField] private Monster greenMonsterPrefab;
    //[SerializeField] private SkullMan skullManPrefab;
    //[SerializeField] private SkullMan greenSkullManPrefab;
    [Header("미리 생성할 개수")]
    [SerializeField] private int monsterCount = 30;
    //[SerializeField] private int greenMonsterCount = 30;
    //[SerializeField] private int skullManCount = 10;
    //[SerializeField] private int greenSkullManCount = 10;

    private void Start()
    {
        Managers.Pool.PreloadPool(monsterPrefab, monsterCount);
        //Managers.Pool.PreloadPool(greenMonsterPrefab, greenMonsterCount);
        //Managers.Pool.PreloadPool(skullManPrefab, skullManCount);
        //Managers.Pool.PreloadPool(greenSkullManPrefab, greenSkullManCount);
    }
}
