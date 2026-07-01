using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;

    [Header("Boss")]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip magic;
    [SerializeField] private AudioClip bullet;
    [SerializeField] private AudioClip bulletHit;
    [SerializeField] private AudioClip poisonBall;
    [SerializeField] private AudioClip poisonField;
    [SerializeField] private AudioClip poisonFieldHit;
    [SerializeField] private AudioClip drop;
    [SerializeField] private AudioClip dropHit;
    [SerializeField] private AudioClip explode;
    [SerializeField] private AudioClip cloning;
    [SerializeField] private AudioClip bossDie;

    [Header("Monster")]
    [SerializeField] private AudioClip monsterAttack;
    [SerializeField] private AudioClip monsterAttackHit;
    [SerializeField] private AudioClip monsterHit;
    [SerializeField] private AudioClip monsterDie;

    [Header("SkullMan")]
    [SerializeField] private AudioClip skullManAttack;
    [SerializeField] private AudioClip skullManHit;
    [SerializeField] private AudioClip skullManDie;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, volume);
    }

    // Boss
    public void PlayHit() => Play(hit);
    public void PlayMagic() => Play(magic);
    public void PlayBullet() => Play(bullet);
    public void PlayBulletHit() => Play(bulletHit);
    public void PlayPoisonBall() => Play(poisonBall);
    public void PlayPoisonField() => Play(poisonField);
    public void PlayPoisonFieldHit() => Play(poisonFieldHit);
    public void PlayDrop() => Play(drop);
    public void PlayDropHit() => Play(dropHit);
    public void PlayExplode() => Play(explode);
    public void PlayCloning() => Play(cloning);
    public void PlayBossDie() => Play(bossDie);

    // Monster
    public void PlayMonsterAttack() => Play(monsterAttack);
    public void PlayMonsterAttackHit() => Play(monsterAttackHit);
    public void PlayMonsterHit() => Play(monsterHit);
    public void PlayMonsterDie() => Play(monsterDie);

    // SkullMan
    public void PlaySkullManAttack() => Play(skullManAttack);
    public void PlaySkullManHit() => Play(skullManHit);
    public void PlaySkullManDie() => Play(skullManDie);


    // Volume Overload
    public void PlayHit(float volume) => Play(hit, volume);
    public void PlayMagic(float volume) => Play(magic, volume);
    public void PlayBullet(float volume) => Play(bullet, volume);
    public void PlayPoisonBall(float volume) => Play(poisonBall, volume);
    public void PlayPoisonField(float volume) => Play(poisonField, volume);
    public void PlayDrop(float volume) => Play(drop, volume);
    public void PlayExplode(float volume) => Play(explode, volume);
    public void PlayCloning(float volume) => Play(cloning, volume);
    public void PlayBossDie(float volume) => Play(bossDie, volume);

    public void PlayMonsterAttack(float volume) => Play(monsterAttack, volume);
    public void PlayMonsterHit(float volume) => Play(monsterHit, volume);
    public void PlayMonsterDie(float volume) => Play(monsterDie, volume);
}