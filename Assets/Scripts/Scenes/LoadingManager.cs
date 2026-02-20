using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;

    [SerializeField]
    private TMP_Text loadingText;

    [SerializeField]
    private string nextSceneName = "SampleScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        // 비동기 로딩 시작 (백그라운드 작업 시작)
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // 로딩이 끝나도 자동으로 씬을 넘기지 않도록 막음.
        // 로딩이 너무 빠를 때 로딩 화면이 휙 지나가는 것을 방지하기 위해서.
        op.allowSceneActivation = false;

        float timer = 0.0f; // 페이크 로딩을 위하여.

        // 로딩이 완료될 때까지 반복.
        while(op.isDone == false)
        {
            timer += Time.deltaTime;

            if(op.progress < 0.9f)
            {
                //loadingBar.fillAmount = op.progress;
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, timer);

                if(loadingText != null)
                {
                    // Mathf.RoundToInt : 반올림해서 정수로 반환해 주는 함수.
                    loadingText.text = "Loaing..." + Mathf.RoundToInt(loadingBar.fillAmount * 100.0f) + "%";
                }

                if(loadingBar.fillAmount >= op.progress)
                {
                    timer = 0.0f;
                }
            }
            else
            {
                //loadingBar.fillAmount = 1.0f;
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1.0f, timer);

                if (loadingText != null)
                {
                    // Mathf.RoundToInt : 반올림해서 정수로 반환해 주는 함수.
                    loadingText.text = "Loaing... 100%";
                }

                if(loadingBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }
    }
}
