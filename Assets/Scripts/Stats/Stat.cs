using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int value;
    
    public Stat(int val)
    {
        value = val;
    }

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    public void SubtractValue(int damage)
    {
        value -= damage;
    }
}
