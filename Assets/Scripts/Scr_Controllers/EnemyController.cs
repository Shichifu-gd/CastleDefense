using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public RouteForEnemies routeForEnemies;
    public GameController gameController;
    public UIController uIController;
    public Spawn spawn;

    private float TimeSpawn;
    private float LastWaveCount;

    private int MaxNumberEnemies;
    private int CurrentNumberEnemies;
    private int NumberSpawn = 0;
    public int NumberOfDead { get; set; }

    private bool SwitchSpawn = true;

    private Transform SpawnPointEnemy;
    public GameObject PreEnemy;

    public ScriptableObjectsLevelListEnemy[] EnemyLevels;
    private ScriptableObjectsEnemy[] SelectedUnits;
    [HideInInspector]
    public List<GameObject> EnemyList;
    private List<float> LevelsDistribution = new List<float>();

    private void Start()
    {
        MaxNumberEnemies = Random.Range(1, 6);
    }

    public void SetSpawnPoint(Transform point)
    {
        SpawnPointEnemy = point;
    }

    public void NextWave()
    {
        if (SwitchSpawn)
        {
            SwitchSpawn = false;
            LastWaveCount = gameController.Score;
            MaxNumberEnemies += Random.Range(4, 7);
            StartEnemySpawn();
        }
    }

    public void StartEnemySpawn()
    {
        UpdateListLevelsDistribution();
        SelectedUnits = new ScriptableObjectsEnemy[MaxNumberEnemies];
        CurrentNumberEnemies = MaxNumberEnemies;
        UnitSelection();
        uIController.AssignValuesEnemyCountText(MaxNumberEnemies.ToString(), MaxNumberEnemies.ToString());
        StartCoroutine(EnemySpawn());
    }

    private void UpdateListLevelsDistribution()
    {
        LevelsDistribution.Clear();
        for (int index = 0; index < EnemyLevels.Length; index++)
        {
            LevelsDistribution.Add(EnemyLevels[index].Distribution.Evaluate(LastWaveCount));
        }
    }

    private void UnitSelection()
    {
        for (int mainIndex = 0; mainIndex < SelectedUnits.Length; mainIndex++)
        {
            float value = Random.Range(0, LevelsDistribution.Sum());
            float sum = 0;
            if (LastWaveCount < 1000)
            {
                for (int addIndex = 0; addIndex < LevelsDistribution.Count; addIndex++)
                {
                    sum += LevelsDistribution[addIndex];
                    if (value < sum)
                    {
                        SelectedUnits[mainIndex] = EnemyLevels[addIndex].EnemyList[Random.Range(0, EnemyLevels[addIndex].EnemyList.Length)];
                        break;
                    }
                }
            }
            else SelectedUnits[mainIndex] = EnemyLevels[EnemyLevels.Length - 1].EnemyList[Random.Range(0, EnemyLevels[EnemyLevels.Length - 1].EnemyList.Length - 1)];
        }
    }

    private IEnumerator EnemySpawn()
    {
        for (int index = 0; index < SelectedUnits.Length; index++)
        {
            NumberSpawn++;
            if (NumberSpawn <= 15) TimeSpawn = FindOutSpawnTime();
            else TimeSpawn = Random.Range(0.8f, 1.1f);
            yield return new WaitForSeconds(TimeSpawn);
            NewEnemy(SelectedUnits[index]);
        }
        SelectedUnits = null;
        SwitchSpawn = true;
    }

    private float FindOutSpawnTime()
    {
        if (NumberSpawn <= 1) return 3f;
        if (NumberSpawn <= 10) return Random.Range(1.4f, 1.8f);
        if (NumberSpawn > 10 && NumberSpawn <= 15) return Random.Range(1f, 1.5f);
        return 1f;
    }

    private void NewEnemy(ScriptableObjectsEnemy enemy)
    {
        GameObject enemyObject = spawn.SpawnEnemy(PreEnemy, SpawnPointEnemy.position);
        enemyObject.GetComponent<Enemy>().AssignValuesForEnemy(enemy, routeForEnemies.NodesList);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnRegisterEnemy(GameObject enemy, int score)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
        gameController.Score += score;
        CurrentNumberEnemies--;
        if (CurrentNumberEnemies >= 0) uIController.AssignValuesEnemyCountText(CurrentNumberEnemies.ToString(), MaxNumberEnemies.ToString());
        CheckForRemainingEnemies();
    }

    public void SetNumberOfDead()
    {
        NumberOfDead++;
    }

    private void CheckForRemainingEnemies()
    {
        if (CurrentNumberEnemies <= 0 && !gameController.OnPause()) gameController.EndWave();
    }

    public void AllStop()
    {
        StopAllCoroutines();
        for (int index = 0; index < EnemyList.Count; index++)
        {
            EnemyList[index].GetComponent<Enemy>().IsDead = true;
        }
    }
}