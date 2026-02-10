using UnityEngine;
using System.IO;    // 파일 입출력 기능을 사용하기 위한 네임스페이스.

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameData currentData;

    private string savePath;

    private void Awake()
    {
        Instance = this;

        // 저장 경로 설정 : 유니티가 제공하는 내부 저장소의 경로 + 저장할 파일의 이름을 합쳐서 최정 경로 문자열을 생성한다.
        savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        // 클래스의 멤버 변수들을 JSON 문자열로 변환 (직렬화)
        string jsonString = JsonUtility.ToJson(currentData, true);

        File.WriteAllText(savePath, jsonString);

        Debug.Log("내부 저장소 경로 : " + savePath);
        Debug.Log("저장된 내용 : " + jsonString);
    }

    public void LoadGame()
    {
        if(File.Exists(savePath) == true)
        {
            string jsonString = File.ReadAllText(savePath);

            currentData = JsonUtility.FromJson<GameData>(jsonString);

            Debug.Log("게임 로드 완료!!!");
        }
        else
        {
            currentData = new GameData();
            Debug.Log("저장 파일이 존재하지 않음.");
        }
    }

    // 게임이 종료될 때 유니티가 자동으로 호출하는 함수.
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
