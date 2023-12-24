using UnityEngine;


public class Enemy : MonoBehaviour
{


    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float defaultSpeed;
    float moveSpeed;
    float targetSpeed;
    bool isAcceleration;

    [SerializeField]
    float rotationSpeed;
    float currentRotationSpeed;

    [SerializeField]
    float health;
    [SerializeField]
    float maxHealth;
    bool isLive;

    [SerializeField]
    BoxCollider2D areaCollider;

    float angle;
    Vector3 currentWaypoint;
    float prevWaypointDistance;
    float waypointDistance;
    bool isComingClose;

    Rigidbody2D rigid;
    new CapsuleCollider2D collider;

    [Header("Accel/Rotate Values")]
    [SerializeField]
    float accelerateAmount = 2.0f;
    float currentAccelerate;

    [SerializeField]
    [Range(0, 1)]
    float evasionRate = 0.5f;
    int hitCount;

    float followRateTimer;
    float followTimer;
    float followDistance;
    bool isFirst = true;
    bool isFollow = false;

    EnemyWeaponController enemyWeaponController;



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        collider = GetComponent<CapsuleCollider2D>();

    }

    void Start()
    {
        areaCollider = GameManager.Instance.player.playerArea;
        enemyWeaponController = GetComponentInChildren<EnemyWeaponController>();
        moveSpeed = defaultSpeed;
        currentRotationSpeed = rotationSpeed;

        Vector3 initialpointPosition = RandomPointInBounds(areaCollider.bounds);

        currentWaypoint = initialpointPosition;

        ChangeWaypoint();
    }

    void OnEnable()
    {
        isLive = true;
        collider.enabled = true;
        rigid.simulated = true;
        health = maxHealth;

        if (!isFirst)
        {
            ChangeWaypoint();

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckWaypoint();
        Rotate();
        AdjustSpeed();
        Move();
    }

    void Move()
    {
        rigid.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    void ChangeWaypoint()
    {

        CreateWaypoint();

        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);
        prevWaypointDistance = waypointDistance;
        RandomizeSpeedAndTurn();

        isComingClose = false;
    }


    void FollowPlayer()
    {
        followTimer += Time.fixedDeltaTime;
        if (followTimer >= 7.0f)
        {
            isFollow = false;
            followTimer = 0f;
            followRateTimer = 0f;
            return;
        }

        currentWaypoint = GameManager.Instance.player.transform.position;
        followDistance = Vector3.Distance(transform.position, currentWaypoint);
        if (followDistance < 5f)
        {
            enemyWeaponController.GunFire();
        }
    }

    void CheckWaypoint()
    {
        if (currentWaypoint == null) return;
        followRateTimer += Time.fixedDeltaTime;
        if (!isFollow && followRateTimer > 5f)
        {
            followRateTimer = 0f;
            float followRate = Random.Range(0.0f, 1.0f);
            if (followRate < 0.15f)
            {
                isFollow = true;
            }
        }

        if (isFollow)
        {
            FollowPlayer();
        }

        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);

        if (waypointDistance >= prevWaypointDistance) // Aircraft is going farther from the waypoint
        {
            if (isComingClose == true)
            {
                ChangeWaypoint();
            }
        }
        else
        {
            isComingClose = true;
        }

        prevWaypointDistance = waypointDistance;
    }

    void CreateWaypoint()
    {

        Vector3 waypointPosition = RandomPointInBounds(areaCollider.bounds);
        //Instantiate(waypointObject, waypointPosition, Quaternion.identity);
        currentWaypoint = waypointPosition;
    }

    void Rotate()
    {
        if (currentWaypoint == null)
            return;

        Vector3 targetDir = (currentWaypoint - transform.position).normalized;
        float hAxis = targetDir.x;
        float vAxis = targetDir.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (Mathf.Abs(zAxis) > 0f)
        {
            angle = Mathf.LerpAngle(angle, -zAxis, Time.fixedDeltaTime * currentRotationSpeed);
            rigid.MoveRotation(Quaternion.Euler(Vector3.forward * angle));
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }

    void RandomizeSpeedAndTurn()
    {
        // Speed
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        isAcceleration = (moveSpeed < targetSpeed);

        // Rotation Speed
        currentRotationSpeed = Random.Range(0.75f, rotationSpeed);
    }

    void AdjustSpeed()
    {
        currentAccelerate = 0;
        if (isAcceleration == true && moveSpeed < targetSpeed)
        {
            currentAccelerate = accelerateAmount;
        }
        else if (isAcceleration == false && moveSpeed > targetSpeed)
        {
            currentAccelerate = -accelerateAmount;
        }
        moveSpeed += currentAccelerate * Time.fixedDeltaTime;

        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, rotationSpeed, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isWeaponHit = other.CompareTag("Bullet") || other.CompareTag("Missile");

        if (!isWeaponHit || !isLive) return;

        if (other.CompareTag("Bullet"))
        {
            health -= other.GetComponent<Bullet>().BulletDamage;
            hitCount++;

            if (hitCount >= 3)
            {
                float rate = Random.Range(0.0f, 1.0f);
                if (rate <= evasionRate)
                {
                    ChangeWaypoint();
                    hitCount = 0;
                }
            }
        }
        else
        {
            health -= other.GetComponent<Missile>().MissileDamage;
        }



        if (health <= 0)
        {
            isLive = false;
            collider.enabled = false;
            rigid.simulated = false;
            isFirst = false;
            hitCount = 0;
            gameObject.SetActive(false);
        }
    }
}
