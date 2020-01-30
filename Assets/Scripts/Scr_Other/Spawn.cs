using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject SlotForEnemys;
    public GameObject SlotForDecors;
    public GameObject SlotForTowers;
    public GameObject SlotForProjectile;
    public GameObject SlotForPoints;

    #region sorting
    public GameObject SpawnPoint(GameObject prefab, Vector2 spawnPoint)
    {
        GameObject newPoint = SpawnObject(prefab, spawnPoint);
        newPoint.transform.parent = SlotForPoints.transform;
        return newPoint;
    }

    public GameObject SpawnDecor(GameObject prefab, Vector2 spawnPoint)
    {
        GameObject newDecor = SpawnObject(prefab, spawnPoint);
        newDecor.transform.parent = SlotForDecors.transform;
        return newDecor;
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector2 spawnPoint)
    {
        GameObject newEnemy = SpawnObject(prefab, spawnPoint);
        newEnemy.transform.parent = SlotForEnemys.transform;
        return newEnemy;
    }

    public GameObject SpawnTower(GameObject prefab, Vector2 spawnPoint)
    {
        GameObject newTower = SpawnObject(prefab, spawnPoint);
        newTower.transform.parent = SlotForTowers.transform;
        return newTower;
    }

    public GameObject SpawnProjectile(GameObject prefab, Vector2 spawnPoint)
    {
        GameObject newProjectile = SpawnObject(prefab, spawnPoint);
        newProjectile.transform.parent = SlotForProjectile.transform;
        return newProjectile;
    }
    #endregion

    public GameObject SpawnObject(GameObject prefab, Vector2 spawnPoint)
    {
        return Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}