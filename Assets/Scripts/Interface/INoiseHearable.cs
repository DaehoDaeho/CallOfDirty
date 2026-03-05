using UnityEngine;

public interface INoiseHearable
{
    /// <summary>
    /// 소음의 위치와 강도를 전달받는 함수.
    /// </summary>
    /// <param name="noisePosition">소음의 위치</param>
    /// <param name="intensity">소음의 강도</param>
    void OnHearNoise(Vector3 noisePosition, float intensity);
}
