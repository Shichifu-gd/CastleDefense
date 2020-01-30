using System.Collections;
using UnityEngine;
using System;

public class TowerController : MonoBehaviour
{
    public TowerPurchase towerPurchase;
    public TowerUpdate towerUpdate;
    public Player player;
    public Spawn spawn;

    private Vector2 CellCoordinates;

    public GameObject PreTower;
    public GameObject PanelForPurchase;
    public GameObject PanelForUpdate;

    public ScriptableObjectsTower[] AccessibleTowers;

    private void Start()
    {
        PanelForPurchase.SetActive(false);
        PanelForUpdate.SetActive(false);
        towerPurchase.SetImages(AccessibleTowers);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (PointCheck(raycastHit2D))
            {
                CellCoordinates = raycastHit2D.collider.GetComponent<Cell>().transform.position;
                if (raycastHit2D.collider.tag == "CellForTower") OnPanel(1);
                else if (raycastHit2D.collider.tag == "CellForTowerFull") OnPanel(2);
                else if (raycastHit2D.collider.tag == "EmptyCell") StartCoroutine(OffPanels());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) StartCoroutine(OffPanels());
    }

    private bool PointCheck(RaycastHit2D raycastHit)
    {
        Cell cell = raycastHit.collider.GetComponent<Cell>();
        if (cell != null)
        {
            if (cell.OnMouse()) return true;
        }
        return false;
    }

    private void OnPanel(int index)
    {
        if (index == 1)
        {
            FollowMouse(PanelForPurchase);
            towerPurchase.SetPointForSpawnTower(CellCoordinates);
            PanelForUpdate.SetActive(false);
            PanelForPurchase.SetActive(true);
        }
        if (index == 2)
        {
            FollowMouse(PanelForUpdate);
            PanelForPurchase.SetActive(false);
            PanelForUpdate.SetActive(true);
            StartCoroutine(towerUpdate.UpdateText());
        }
    }

    public IEnumerator OffPanels()
    {
        yield return new WaitForSeconds(.01f);
        PanelForPurchase.SetActive(false);
        PanelForUpdate.SetActive(false);
    }

    public void FollowMouse(GameObject panel)
    {
        panel.transform.position = CellCoordinates;
    }

    public void SpawnTower(int index, Cell cell)
    {
        Cell currentCell = cell;
        int gold = -Math.Abs(AccessibleTowers[index].PriceTower);
        if (cell.tag == "CellForTower" && player.Gold >= AccessibleTowers[index].PriceTower)
        {
            StartCoroutine(OffPanels());
            GameObject newTower = spawn.SpawnTower(PreTower, CellCoordinates);
            newTower.GetComponentInChildren<Tower>().TowerComponents(AccessibleTowers[index]);
            newTower.GetComponentInChildren<Tower>().SetTowerLevel(AccessibleTowers[index].Levels);
            player.SetGold(gold);
            cell.tag = "CellForTowerFull";
        }
        else return;
    }
}