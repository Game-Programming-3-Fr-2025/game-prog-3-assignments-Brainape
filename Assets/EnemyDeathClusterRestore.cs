using UnityEngine;

public class EnemyDeathClusterRestore : MonoBehaviour
{
    [SerializeField] private int radiationRestoreAmount = 50; // how much this enemy gives back

    private void OnDestroy()
    {
        // Find clusters touching this enemy on death
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("NukeCluster"))
            {
                NukeCluster cluster = hit.GetComponent<NukeCluster>();
                if (cluster != null)
                {
                    cluster.RegainRadiation(radiationRestoreAmount);
                }
            }
        }
    }
}
