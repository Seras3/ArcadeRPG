public class ShotgunBulletPool : ObjectPool
{
    public static ShotgunBulletPool instance;

    // Start is called before the first frame update
    private void Awake() {
        if (instance == null){
            instance = this;
        }
    }

    private void Start() {
        AddObjects(amountToPool);
    }
}