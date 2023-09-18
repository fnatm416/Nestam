using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance = null;

    public GameObject[] prefabs;
    Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        for (int index = 0; index < prefabs.Length; index++)
        {
            pools[prefabs[index].name] = new List<GameObject>();
        }
    }

    public GameObject Get(string name)
    {
        GameObject select = null;

        foreach (GameObject item in pools[name])
        {
            //놀고있는 오브젝트를 select에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //만약 놀고있는 오브젝트를 못찾았다면 새롭게 생성
        if (!select)
        {
            for (int index = 0; index < prefabs.Length; index++)
            {
                if (prefabs[index].name == name)
                {
                    select = Instantiate(prefabs[index], this.transform);
                    //새롭게 생성된 오브젝트를 풀에 등록
                    pools[prefabs[index].name].Add(select);
                }
            }
        }

        return select;
    }

    public void Return(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(this.transform);
    }
}