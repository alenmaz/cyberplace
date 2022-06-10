using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolSettings
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amount;
    [SerializeField] private GameObject parent;

    public GameObject Prefab { get => prefab; }
    public int Amount { get => amount; }

    public GameObject Parent { get => parent; }
}

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] PoolSettings[] poolSettings;
    private List<GameObject> pool;
    private bool isCreated = false;

    private void Awake()
    {
        if (!isCreated)
        {
            DontDestroyOnLoad(gameObject);
            pool = new List<GameObject>();

            GameObject tmp;
            foreach (var poolSetting in poolSettings)
                for (int i = 0; i < poolSetting.Amount; i++)
                {
                    tmp = Instantiate(poolSetting.Prefab);
                    tmp.SetActive(false);
                    tmp.transform.parent = poolSetting.Parent.transform;
                    pool.Add(tmp);
                }
            isCreated = true;
        }
    }

    public GameObject GetInstance(ObjectType type)
    {
        if(pool.Count == 0)
        {
            Debug.Log("The enemy pool is empty");
            return null;
        }
        var obj = pool.Find(x => x.GetComponent<PoolableObject>().ObjectType.Equals(type) && !x.active);
        if (obj == null) return null;
        return obj;
    }

    public void ReturnInstance(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        pool.Add(obj);
    }

    private void OnLevelWasLoaded(int level)
    {
        foreach(var obj in pool)
            obj.SetActive(false);
    }
}
