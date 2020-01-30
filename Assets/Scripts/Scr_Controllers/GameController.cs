using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public EnemyController enemyController;
    public TrailGeneration trailGeneration;
    public UIController uIController;
    public Player player;

    private int Wave;
    private int CurrentWave;

    public float Score { get; set; }

    private bool Pause;

    private void Start()
    {
        AssignComponents();
        StartCoroutine(trailGeneration.Generation());
    }

    private void AssignComponents()
    {
        Wave = Random.Range(5, 13);
        CurrentWave = 0;
        Score = 0;
    }

    public void StartGame()
    {
        uIController.OnPanel();
        NexWave();
    }

    public void EndWave()
    {
        if (CurrentWave < Wave) uIController.SwitchPanelNextWave(true);
        else uIController.SwitchPanelRestart();
    }
    public void EndGame()
    {
        enemyController.AllStop();
        uIController.SwitchPanelRestart();
        uIController.EnemyCountDead(enemyController.NumberOfDead.ToString());
        SetPause();
    }

    public void NexWave()
    {
        CurrentWave++;
        uIController.AssignValuesWaveText(CurrentWave.ToString(), Wave.ToString());
        uIController.SwitchPanelNextWave(false);
        StartEnemySpawn();
    }

    private void StartEnemySpawn()
    {
        enemyController.NextWave();
    }

    public bool OnPause()
    {
        return Pause;
    }

    public void SetPause()
    {
        Pause = !Pause;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}