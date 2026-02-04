using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource bgmSource;

    [SerializeField]
    private AudioSource sfxSource;

    private void Awake()
    {
        instance = this;

        // 씬이 파괴되어도 오브젝트가 파괴되지 않도록 해준다.
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    /// <param name="clip">사운드 파일</param>
    /// <param name="volume">볼륨. 만약 함수를 호출하는 쪽에서 값을 지정하지 않을 경우 자동으로 1이 지정된다</param>
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if(clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if(clip == null)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.loop = true;  // 반복재생.
        bgmSource.Play();
    }
}
