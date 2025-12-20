using UnityEngine;

public class EnemyDeathClusterRestore : MonoBehaviour
{
    [SerializeField] private int radiationRestoreAmount = 50; 
    private void OnDestroy()
    {
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
