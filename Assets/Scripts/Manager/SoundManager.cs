using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    //Melee가 2개이기때문에 Hit부터 값 지정
    public enum Sfx 
    { 
        Hit, 
        Start = 2,
        Win,
        Lose
    }
    public enum Bgm
    {
        Title,
        Intro,
        Select,
        Play,
        Ending
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Init();
    }

    public void Init()
    {
        //배경음악, 효과음 사운드 정보가 둘중 하나라도 없으면 그냥 초기화
        if (PlayerPrefs.HasKey("Bgm") && PlayerPrefs.HasKey("Sfx"))
        {
            bgmVolume = PlayerPrefs.GetFloat("Bgm");
            sfxVolume = PlayerPrefs.GetFloat("Sfx");
        }

        //bgm 초기화
        GameObject bgmObject = new GameObject("bgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[0];

        //sfx 초기화
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    //배경음악 재생
    public void PlayBgm(Bgm bgm)
    {
        bgmPlayer.clip = bgmClips[(int)bgm];
        bgmPlayer.Play();
    }

    //효과음 재생
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            //쉬고있는 채널에서 효과음을 재생
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            //공격사운드는 2개중에 랜덤재생
            if (sfx == Sfx.Hit)
                ranIndex = Random.Range(0, 2);  //0~1

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}