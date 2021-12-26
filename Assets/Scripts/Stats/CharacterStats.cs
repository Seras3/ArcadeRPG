using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public abstract class CharacterStats : MonoBehaviour
{
    protected int MaxHealth = 100;

    public Stat CurrentHealth { get; set; }
    public float MovementSpeed { get; set; }

    [SerializeField] protected GameObject sliderPrefab;
    [SerializeField] protected Vector3 SliderOffset = new Vector3(0, 3, 0);
    [SerializeField] protected Canvas parentCanvas;

    [SerializeField] protected bool hasHpBar = true;

    [SerializeField] protected float regenDelay, regenInterval;
    [SerializeField] protected int regenValue;
    [SerializeField] protected bool hasRegen;
    private int lastRegenCall;


    protected Slider slider;
    protected GameObject sliderGO;

    private Plane cameraPlane;

    protected void Awake()
    {
        sliderPrefab = Resources.Load("HpSlider") as GameObject;
        parentCanvas = GameObject.Find("HPBarCanvas").GetComponent<Canvas>();

        CurrentHealth = new Stat(MaxHealth);
        MovementSpeed = 0.005f;

        HPbarSetup();
    }

    protected void LateUpdate() {
        HPbarFollow();
    }

    private void OnDisable() { // sets inactive on inactive parent object
        setHpBarActive(); 
    }

    private void OnEnable() { // active on active parent
        setHpBarActive();
    }

    protected void HPbarSetup(){
        if (!hasHpBar) return;

        //hp bar setup is called within child class; called once at start
        sliderGO = Instantiate(sliderPrefab, parentCanvas.transform);
        slider = sliderGO.GetComponent<Slider>();
        sliderGO.transform.position = this.gameObject.transform.position + SliderOffset;
        sliderGO.transform.LookAt(GeometryUtility.CalculateFrustumPlanes(Camera.main)[4].ClosestPointOnPlane(sliderGO.transform.position), Vector3.up); // perpendicular to the camera plane
    }

    protected void HPbarFollow(){
        if (!hasHpBar) return;

        //the hp bar follows the character and updates itself
        sliderGO.transform.position = this.gameObject.transform.position + SliderOffset;
        slider.value = (float)CurrentHealth.GetValue()/(float)MaxHealth;
        //sliderGO.transform.LookAt(Camera.main.transform, Vector3.up);
    }

    protected void setHpBarActive(){
        if (!hasHpBar || this.gameObject == null || sliderGO== null) return;

        sliderGO.SetActive(this.gameObject.activeInHierarchy);
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth.GetValue() > 0)
        {
            CurrentHealth.SubtractValue(damage);
            //Debug.Log(transform.name + " takes " + damage + " damage.");
            if (hasRegen){
                StartCoroutine(RegenAfterDelay(regenValue, regenInterval, regenDelay));
            }
        }

        if (CurrentHealth.GetValue() <= 0)
        {
            Die();
        }
    }

    protected IEnumerator RegenAfterDelay(int value, float interval, float delay){
        lastRegenCall+=1;
        int callIndex = lastRegenCall;

        yield return new WaitForSeconds(regenDelay);
        
        while (callIndex == lastRegenCall){ // only the last RegenAfterDelay called in the stack has the right to regen
            CurrentHealth.SetValue(Mathf.Min(CurrentHealth.GetValue()+value, MaxHealth));
            yield return new WaitForSeconds(regenInterval);
        }
    }

    public abstract void Die();

    public void InitStats()
    {
        CurrentHealth.SetValue(MaxHealth);
    }
}
