using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    float timer;

    [Header("# Bullet Position")]
    [SerializeField]
    Transform enemyBulletLeftPos;
    [SerializeField]
    Transform enemyBulletRightPos;
    void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }



    public void GunFire()
    {
        // OverHeatControll();
        // if (!isFiring || isOverheat) return;

        timer += Time.deltaTime;
        if (timer > 0.2)
        {
            timer = 0f;
            FireMachineGun();
        }
    }

    void FireMachineGun()
    {
        Transform bulletLeft = GameManager.Instance.pool.Get(1).transform;
        Transform bulletRight = GameManager.Instance.pool.Get(1).transform;
        bulletLeft.position = enemyBulletLeftPos.position;
        bulletRight.position = enemyBulletRightPos.position;
        bulletLeft.rotation = enemyBulletLeftPos.rotation;
        bulletRight.rotation = enemyBulletRightPos.rotation;
        bulletLeft.GetComponent<EnemyBullet>().Init(5);
        bulletRight.GetComponent<EnemyBullet>().Init(5);
    }
}
