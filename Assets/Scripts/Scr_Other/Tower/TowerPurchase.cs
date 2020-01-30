using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TowerPurchase : MonoBehaviour
{
    private TowerController towerController;
    private Cell cell;

    private Vector2 PointForSpawnTower;

    public Image[] ImageTower;
    public TMP_Text[] Price;

    private void Awake()
    {
        towerController = GetComponentInParent<TowerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cell = collision.GetComponent<Cell>();
    }

    public void SetImages(ScriptableObjectsTower[] towers)
    {
        for (int index = 0; index < ImageTower.Length; index++)
        {
            ImageTower[index].sprite = towers[index].SpriteForIco;
            Price[index].text = towers[index].PriceTower.ToString();
        }
    }

    public void SetPointForSpawnTower(Vector2 point)
    {
        PointForSpawnTower = point;
    }

    public void NewTower(int index)
    {
        towerController.SpawnTower(index, cell);
    }
}