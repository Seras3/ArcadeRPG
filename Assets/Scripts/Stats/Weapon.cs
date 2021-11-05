using UnityEngine;

public class Weapon
{
    public string Name { get; }

    public int Damage { get; }
    //public int ammo;

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }
}
