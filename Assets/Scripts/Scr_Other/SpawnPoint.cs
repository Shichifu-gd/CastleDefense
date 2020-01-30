using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        EnemyController enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        enemyController.SetSpawnPoint(gameObject.transform);
    }
}