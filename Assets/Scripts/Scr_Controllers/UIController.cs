using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text WaveText;
    public TMP_Text GoldText;
    public TMP_Text LifeText;
    public TMP_Text EnemyCountText;
    public TMP_Text EnemyCountDeadText;

    public GameObject PanelMain;
    public GameObject PanelRestart;
    public GameObject PanelNextWave;

    private void Start()
    {
        PanelMain.SetActive(false);
        PanelRestart.SetActive(false);
        PanelNextWave.SetActive(false);
    }

    public void AssignValuesGoldText(string gold)
    {
        GoldText.text = gold;
    }

    public void AssignValuesLifeText(string life)
    {
        LifeText.text = life;
    }

    public void AssignValuesEnemyCountText(string current, string max)
    {
        EnemyCountText.text = $"{current} / {max}";
    }

    public void AssignValuesWaveText(string current, string max)
    {
        WaveText.text = $"{current} / {max}";
    }

    public void OnPanel()
    {
        PanelMain.SetActive(true);
    }

    public void SwitchPanelNextWave(bool option)
    {
        PanelNextWave.SetActive(option);
    }

    public void SwitchPanelRestart()
    {
        PanelRestart.SetActive(true);
    }

    public void EnemyCountDead(string current)
    {
        EnemyCountDeadText.text = $"Count dead: {current}";
    }
}