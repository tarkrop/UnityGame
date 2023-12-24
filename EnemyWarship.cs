using UnityEngine;

public class EnemyWarship : MonoBehaviour
{
    bool isLive;
    [SerializeField]
    float maxHealth;
    float health;

    [SerializeField]
    float moveSpeed;
    float angle;



    Rigidbody2D rigid;
    new CapsuleCollider2D collider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        collider = GetComponent<CapsuleCollider2D>();

    }

    void OnEnable()
    {
        angle = Random.Range(-180, 180);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        isLive = true;
        collider.enabled = true;
        rigid.simulated = true;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rigid.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isWeaponHit = other.CompareTag("Bullet") || other.CompareTag("Missile");

        if (other.CompareTag("Area")) CancelInvoke("DestroyWarship");
        if (!isWeaponHit || !isLive) return;

        if (other.CompareTag("Bullet"))
        {
            health -= other.GetComponent<Bullet>().BulletDamage;
        }
        else
        {
            health -= other.GetComponent<Missile>().MissileDamage;
        }

        if (health <= 0)
        {
            DestroyWarship();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }
        Invoke("DestroyWarship", 20);
    }

    void DestroyWarship()
    {
        isLive = false;
        collider.enabled = false;
        rigid.simulated = false;
        gameObject.SetActive(false);
    }


}
