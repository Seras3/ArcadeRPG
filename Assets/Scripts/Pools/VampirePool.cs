using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirePool : ObjectPool
{
    public static VampirePool instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AddObjects(amountToPool);
    }
}
