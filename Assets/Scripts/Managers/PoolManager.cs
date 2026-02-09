using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 직렬화 클래스.
/// 멤버 변수들이 인스펙터 창에 노출되게 하기 위해 직렬화로 만든다.
/// </summary>
[System.Serializable]
public class Pool
{
    public string tag;  // 풀의 이름.
    public GameObject prefab;   // 생성할 프리팹.
    public int size;    // 미리 생성할 개수.
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField]
    private List<Pool> pools;   // 인스펙터에서 설정할 풀 목록.

    [SerializeField]
    private Dictionary<string, Queue<GameObject>> poolDictionary;   // 실제 오브젝트들을 담을 창고 (이름, 오브젝트들의 큐)

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //for(int i=0; i<pools.Count; ++i)
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i<pool.size; ++i)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // 해당 태그의 풀이 존재하는지 확인.
        if(poolDictionary.ContainsKey(tag) == false)
        {
            return null;
        }

        // 큐에서 하나 꺼냄.
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // 활성화 및 위치와 회전 초기화.
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // 사용하려는 객체에 초기화 코드가 있다면 여기서 실행.

        // 다 쓴 것을 다시 큐의 맨 뒤로 보냄. (재사용 준비)
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
