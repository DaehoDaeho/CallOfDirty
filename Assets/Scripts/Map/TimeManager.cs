using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 24)]
    private float currentTime = 12.0f;  // 현재 시간 (0 ~ 24)

    [SerializeField]
    private float dayDuration = 120.0f; // 하루를 몇 초로 지정할 것인가?

    [SerializeField]
    private Light sunLight; // 태양 역할을 할 조명.

    // Update is called once per frame
    void Update()
    {
        // 시간 흐름 계산.
        currentTime += (Time.deltaTime / dayDuration) * 24.0f;
        if(currentTime >= 24.0f)
        {
            currentTime = 0.0f;
        }

        // 태양의 각도 계산
        float sunRotation = (currentTime / 24.0f) * 360.0f;

        // 태양 회전. 90도를 빼서 12(정오)일 때 태양이 머리 위에 오도록 보정.
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90.0f, 170.0f, 0.0f);

        // 밤낮에 따른 빛의 강도 및 색상 변경.
        // 태양이 지평선 아래로 내려가면(밤이 되면) 빛을 끄거나 강도를 낮춤.
        float intensity = Mathf.Clamp01(Vector3.Dot(sunLight.transform.forward, Vector3.down));
        sunLight.intensity = intensity;
    }
}
