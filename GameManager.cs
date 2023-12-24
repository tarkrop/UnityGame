using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# Game Object")]

    public Player player;
    public PoolManager pool;

    [Header("# Object Contorller")]
    public EnemySpawnController enemySpawnController;
    public ScanController scanController;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {

    }
}
