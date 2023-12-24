using UnityEngine;

public class WeaponController : MonoBehaviour
{
    float machinegunTimer;
    float missileTimer;

    float overheatTimer;
    bool isOverheat;

    [SerializeField]
    float missileDelayTime = 3f;

    WaitForSeconds missileDelay;

    [Header("# Bullet Position")]
    [SerializeField]
    Transform bulletLeftPos;
    [SerializeField]
    Transform bulletRightPos;
    [SerializeField]
    Transform missilePos;

    Player player;
    ScanController scanController;
    void Awake()
    {
        player = GameManager.Instance.player;
        scanController = GameManager.Instance.scanController;
        missileDelay = new WaitForSeconds(missileDelayTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GunFire();
        MissileFire();
    }



    void GunFire()
    {
        machinegunTimer += Time.fixedDeltaTime;
        if (machinegunTimer > 0.1f)
        {
            machinegunTimer = 0f;
            FireMachineGun();
        }
    }

    void MissileFire()
    {
        missileTimer += Time.fixedDeltaTime;
        if (missileTimer > 2f)
        {
            if (scanController.nearestTarget == null)
            {
                return;
            }
            else
            {
                missileTimer = 0f;
                FireMissile();
            }
        }
    }


    void OverHeatControll()
    {
        overheatTimer += Time.deltaTime;
        if (overheatTimer > 3f && isOverheat)
        {
            isOverheat = false;
            overheatTimer = 0f;
        }
        else if (overheatTimer > 5f)
        {
            isOverheat = true;
            overheatTimer = 0f;
        }
    }

    void FireMachineGun()
    {
        Transform bulletLeft = GameManager.Instance.pool.Get(0).transform;
        Transform bulletRight = GameManager.Instance.pool.Get(0).transform;
        bulletLeft.position = bulletLeftPos.position;
        bulletRight.position = bulletRightPos.position;
        bulletLeft.rotation = player.transform.rotation;
        bulletRight.rotation = player.transform.rotation;
        //bullet.rotation = Quaternion.FromToRotation(Vector3.up, bulletDir);
        bulletLeft.GetComponent<Bullet>().Init(10);
        bulletRight.GetComponent<Bullet>().Init(10);
    }

    void FireMissile()
    {
        Transform missileTransform = GameManager.Instance.pool.Get(2).transform;
        missileTransform.position = missilePos.position;
        missileTransform.rotation = player.transform.rotation;
        missileTransform.GetComponent<Missile>().Init(300);
    }

    public void PointerDown()
    {

    }
    public void PointerUp()
    {

        overheatTimer = 0f;
    }
}
