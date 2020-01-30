using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TowerUpdate : MonoBehaviour
{
    private TowerController towerController;
    public Player player;
    public Tower tower;
    public Cell cell;

    public TMP_Text TextSalePrice;
    public TMP_Text TextLevelUpPrice;

    public Image Ico;
    public Color DefaultColor;
    public Color AddColor;

    private void Awake()
    {
        towerController = GetComponentInParent<TowerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Tower>()) tower = collision.GetComponent<Tower>();
        if (collision.GetComponent<Cell>()) cell = collision.GetComponent<Cell>();
    }

    public IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(.02f);
        if (tower != null)
        {
            if (tower.TowerLevel < tower.TowerLevels.Length)
            {
                Ico.color = DefaultColor;
                TextSalePrice.text = tower.SalePrice.ToString();
                TextLevelUpPrice.text = tower.PriceForUpgrade.ToString();
            }
            else
            {
                Ico.color = AddColor;
                TextLevelUpPrice.text = "";
            }
        }
    }

    public void UpdateTower()
    {
        if (tower != null)
        {
            bool start = tower.CanIncrease();
            if (player.Gold >= tower.PriceForUpgrade && start)
            {
                int gold = -Mathf.Abs(tower.PriceForUpgrade);
                tower.UpLevelTower();
                player.SetGold(gold);
            }
        }
        StartCoroutine(towerController.OffPanels());
    }

    public void RemoveTower()
    {
        if (tower != null)
        {
            RefundGold();
            cell.gameObject.tag = "CellForTower";
            Destroy(tower.ThisTower);
        }
        StartCoroutine(towerController.OffPanels());
    }

    private void RefundGold()
    {
        player.SetGold(tower.SalePrice);
    }
}