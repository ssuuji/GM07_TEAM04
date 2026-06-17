using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 실제 싱글톤 객체를 저장하는 변수
    private static T instance;
    // 외부제서 접근할 프로퍼티
    public static T Instance
    {
        get
        {
            // 객체가 없다면
            if (instance == null)
            {
                // 현재 씬에서 T 타입을 찾아 저장
                instance = FindFirstObjectByType<T>();
                // 씬에도 객체가 없다면
                if (instance == null)
                {
                    // 새 객체를 생성
                    GameObject obj = new GameObject(typeof(T).Name);
                    // T 컴포넌트 추가
                    instance = obj.AddComponent<T>();
                }
            }
            // 찾거나 생성한 객체 반환
            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 아직 싱글톤 객체가 없다면
        if (instance == null)
        {
            // 현재 객체를 T 타입으로 변환 후 싱글톤으로 등록
            instance = this as T;
            // 씬이 바뀌어도 삭제되지 않도록 해주는 메서드
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 해당 타입의 싱글톤이 존재한다면
            if (instance != this)
            {
                // 새로 만들어지는 객체 제거
                Destroy(gameObject);
            }
        }
    }

    /*=============== Method ===============*/


}