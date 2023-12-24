using System.Collections;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField]
    float missileDamage;

    [SerializeField]
    float rotationSpeed;

    float angle;
    float moveSpeed;

    Transform target;

    [SerializeField]
    float missileLifeTime;
    WaitForSeconds missileLifeTimer;

    Rigidbody2D rigid;
    TrailRenderer trailRenderer;

    Player player;

    public float MissileDamage
    {
        get { return missileDamage; }
    }

    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    void Start()
    {
        player = GameManager.Instance.player;
        missileLifeTimer = new WaitForSeconds(missileLifeTime);
    }

    public void Init(float speed)
    {
        moveSpeed = speed;
    }

    void FixedUpdate()
    {
        MissileMove();
        MissileRotate();
    }

    void MissileMove()
    {
        rigid.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    void MissileRotate()
    {
        target = player.transform;

        Vector3 targetDir = (target.position - transform.position).normalized;
        float hAxis = targetDir.x;
        float vAxis = targetDir.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (Mathf.Abs(zAxis) > 0f)
        {
            angle = Mathf.LerpAngle(angle, -zAxis, Time.fixedDeltaTime * rotationSpeed);
            rigid.MoveRotation(Quaternion.Euler(Vector3.forward * angle));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        gameObject.SetActive(false);
    }


    void OnEnable()
    {
        trailRenderer.Clear();
        StartCoroutine(MissileLifeTime());
    }

    IEnumerator MissileLifeTime()
    {
        yield return missileLifeTimer;
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        target = null;
    }
}
