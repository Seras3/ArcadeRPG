using System;
using Stats;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name;
    public GameObject Bullet;

    public Vector3 OffsetPosition;
    
    public int AmmoCount;

    public int AmmoLimit;

    public bool HasInfiniteAmmo;

    public float FireRate;

    public void ReduceAmmo()
    {
        AmmoCount -= 1;
    }

    public bool HasAmmo() 
    {
        return HasInfiniteAmmo || (AmmoCount > 0);
    }

    public float accuracy;
}
