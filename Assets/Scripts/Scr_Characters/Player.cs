using UnityEngine;

public class Player : MonoBehaviour
{
    private GameController gameController;
    public UIController uIController;

    public int Gold { get; set; }
    public int Life { get; set; }

    private void Awake()
    {
        gameController = GetComponentInParent<GameController>();
    }

    private void Start()
    {
        SetGold(Random.Range(50, 70));
        GetLife(3);
    }

    public void SetGold(int gold)
    {
        Gold += gold;
        uIController.AssignValuesGoldText(Gold.ToString());
    }

    public void GetLife(int life)
    {
        Life += life;
        if (Life < 0) Life = 0;
        uIController.AssignValuesLifeText(Life.ToString());
        if (Life <= 0) gameController.EndGame();
    }
}