using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{

    protected List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] protected int amountToPool = 5;
    [SerializeField] protected int amountToPoolExtra = 5;

    [SerializeField] protected int addMoreTrigger = 2;

    [SerializeField] protected GameObject objectPrefab;
    // Start is called before the first frame update

    protected void AddObjects(int objectsToAdd){
        for (int i = 0; i < objectsToAdd; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
    // private void Start() {

    //     //AddObjects(amountToPool);         
    // }

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
