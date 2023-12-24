using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField]
    float spawnDelaySecond;
    float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnDelaySecond)
        {
            timer = 0f;
            SpawnEnemy();
            float spawnRate = Random.Range(0f, 1f);
            if (spawnRate <= 0.1f)
            {
                SpawnWarship();
            }
            if (spawnRate <= 0.33f)
            {
                SpawnMissile();
            }
        }
    }

    void SpawnEnemy()
    {
        Transform enemy = GameManager.Instance.pool.Get(4).transform;
        float randomX = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        float randomY = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        enemy.position = GameManager.Instance.player.transform.position;
        enemy.position += new Vector3(randomX * 8, randomY * 8, 0);
    }

    void SpawnMissile()
    {
        Transform missile = GameManager.Instance.pool.Get(3).transform;
        float randomX = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        float randomY = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        missile.position = GameManager.Instance.player.transform.position;
        missile.position += new Vector3(randomX * 5, randomY * 5, 0);
        missile.GetComponent<EnemyMissile>().Init(300);
    }

    void SpawnWarship()
    {
        Transform warship = GameManager.Instance.pool.Get(5).transform;
        float randomX = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        float randomY = Random.Range(0, 1) >= 0.5f ? 1f : -1f;
        warship.position = GameManager.Instance.player.transform.position;
        warship.position += new Vector3(randomX * 8, randomY * 8, 0);

    }
}
