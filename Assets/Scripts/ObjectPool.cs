using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 5;
    private int amountToPoolExtra = 5;

    [SerializeField] private int addMoreTrigger;

    [SerializeField] private GameObject bulletPrefab;
    // Start is called before the first frame update
    private void Awake() {
        addMoreTrigger = 4;
        if (instance == null){
            instance = this;
        }
    }

    private void AddObjects(int objectsToAdd){
        for (int i = 0; i < objectsToAdd; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
    private void Start() {

        AddObjects(amountToPool);         
    }

    public GameObject GetPooledObject(){

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (i == pooledObjects.Count - addMoreTrigger){
                AddObjects(amountToPoolExtra);
            }
            
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
