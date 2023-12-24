using UnityEngine;

public class ScanController : MonoBehaviour
{
    [SerializeField]
    float scanRange;
    [SerializeField]
    LayerMask targetlayer;
    public RaycastHit2D[] targets;
    [SerializeField]
    public Transform nearestTarget;

    public bool isLockOn;

    Player player;



    void Start()
    {
        player = GameManager.Instance.player;
    }
    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetlayer);
        nearestTarget = GetTargetTransfrom();
    }

    Transform GetTargetTransfrom()
    {
        Transform result = null;
        float distance = 100f;
        foreach (RaycastHit2D target in targets)
        {

            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            float hAxis = targetDir.x;
            float vAxis = targetDir.y;
            float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
            bool isScanRange = Mathf.Abs(zAxis - player.transform.rotation.z) <= 90f;
            float currentDistance = Vector3.Distance(transform.position, targetPos);

            Debug.Log(Mathf.Abs(zAxis - player.transform.rotation.z));
            if (currentDistance <= distance && isScanRange)
            {
                distance = currentDistance;
                result = target.transform;
            }
        }

        return result;
    }
}
