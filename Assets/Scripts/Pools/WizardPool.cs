namespace Pools
{
    public class WizardPool : ObjectPool
    {
        public static WizardPool instance;

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
}