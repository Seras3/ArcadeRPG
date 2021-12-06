using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterStats : MonoBehaviour
{
    protected int MaxHealth = 100;

    public Stat CurrentHealth { get; set; }
    public float MovementSpeed { get; set; }

    [SerializeField] protected GameObject sliderPrefab;
    [SerializeField] protected Vector3 SliderOffset;
    [SerializeField] protected Canvas parentCanvas;

    protected Slider slider;
    protected GameObject sliderGO;

    protected void Awake()
    {
        CurrentHealth = new Stat(MaxHealth);
        MovementSpeed = 0.005f;
    }

    protected void HPbarSetup(){
        //hp bar setup is called within child class; called once at start
        sliderGO = Instantiate(sliderPrefab, parentCanvas.transform);
        slider = sliderGO.GetComponent<Slider>();
    }

    protected void HPbarFollow(){
        //the hp bar follows the character and updates itself
        sliderGO.transform.position = this.gameObject.transform.position + SliderOffset;
        slider.value = (float)CurrentHealth.GetValue()/(float)MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth.GetValue() > 0)
        {
            CurrentHealth.SubtractValue(damage);
            Debug.Log(transform.name + " takes " + damage + " damage.");
        }

        if (CurrentHealth.GetValue() <= 0)
        {
            Die();
        }
    }

    public abstract void Die();

    public void InitStats()
    {
        CurrentHealth.SetValue(MaxHealth);
    }
}
