public class PistolBulletPool : ObjectPool
{
    public static PistolBulletPool instance;

    private void Awake() {
        if (instance == null){
            instance = this;
        }
    }

    private void Start() {
        AddObjects(amountToPool);
    }
}
