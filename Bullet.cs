using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    float bulletDamage;


    Rigidbody2D rigid;
    TrailRenderer trailRenderer;

    public float BulletDamage
    {
        get { return bulletDamage; }
    }

    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();

    }

    public void Init(float speed)
    {
        rigid.velocity = transform.up * speed;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        trailRenderer.Clear();
    }
}
