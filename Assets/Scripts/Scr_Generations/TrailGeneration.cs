using System.Collections;
using UnityEngine;

public class TrailGeneration : MonoBehaviour
{
    private DeterminantVectorCurrentPosition directions;
    private ComponentsForGeneratingTrails components;
    private GameController gameController;
    public RouteForEnemies routeForEnemies;
    public CellController cellController;
    public Spawn spawn;

    public GameObject VectorDeterminant;
    private GameObject DirectionOfVerificationNorth;
    private GameObject DirectionOfVerificationSouth;
    private GameObject DirectionOfVerificationEast;

    private int NumberForMaximumRandomNumber = 10;
    private int NumberOfCellsTraversedEast;
    private int YPosition = 0;

    private string CurrentForbiddenDirection;

    private bool CompleteShutdown;
    private bool FirstCell;

    private void Awake()
    {
        components = GetComponent<ComponentsForGeneratingTrails>();
        directions = VectorDeterminant.GetComponent<DeterminantVectorCurrentPosition>();
        gameController = GetComponentInParent<GameController>();
    }

    public IEnumerator Generation()
    {
        if (FirstCell == false) FirstStep();
        while (!CompleteShutdown)
        {
            MovingVectorDeterminant();
            VectorDeterminant.SetActive(true);
            yield return new WaitForSeconds(0.02f);
            PavingTrail();
            if (CompleteShutdown) break;
            RefreshCells();
            AddTerritoryForTowers();
            VectorDeterminant.SetActive(false);
        }
    }

    private void FirstStep()
    {
        GameObject newPoint = spawn.SpawnPoint(components.PreSpawnPosition, gameObject.transform.position);
        routeForEnemies.SetStartPoint(newPoint.GetComponent<PathFinder>());
    }

    private void MovingVectorDeterminant()
    {
        Vector2 vector = GetVector();
        VectorDeterminant.transform.position = new Vector2(vector.x, vector.y);
    }

    private Vector2 GetVector()
    {
        Vector2 vector = new Vector2();
        int randomNumber;
        if (NumberOfCellsTraversedEast == 0 && FirstCell == true)
        {
            randomNumber = Random.Range(0, 3);
            if (YPosition <= -2 && randomNumber != 2)
            {
                if (CurrentForbiddenDirection == "east") randomNumber = 0;
                else randomNumber = 2;
            }
            if (YPosition >= 2 && randomNumber != 2)
            {
                if (CurrentForbiddenDirection == "east") randomNumber = 1;
                else randomNumber = 2;
            }
            if (randomNumber == 0 && CurrentForbiddenDirection == "south" || randomNumber == 1 && CurrentForbiddenDirection == "north") randomNumber = 2;
            if (randomNumber == 0)
            {
                vector = directions.North.transform.position;
                CurrentForbiddenDirection = "north";
                NumberOfCellsTraversedEast = 0;
            }
            if (randomNumber == 1)
            {
                vector = directions.South.transform.position;
                CurrentForbiddenDirection = "south";
                NumberOfCellsTraversedEast = 0;
            }
            if (randomNumber == 2)
            {
                vector = directions.East.transform.position;
                CurrentForbiddenDirection = "east";
                NumberOfCellsTraversedEast++;
            }
            HeightCount(randomNumber);
        }
        else
        {
            vector = directions.East.transform.position;
            NumberOfCellsTraversedEast = 0;
            FirstCell = true;
        }
        return vector;
    }

    private void HeightCount(int numSide)
    {
        if (numSide == 0) YPosition++;
        if (numSide == 1) YPosition--;
    }

    private void PavingTrail()
    {
        if (directions.CurrentCell.GetComponent<Cell>().EndOfRoad == false) ChangeCell(directions.CurrentCell, components.Trail, "trail");
        else OffGeneration();
    }

    private void OffGeneration()
    {
        CompleteShutdown = true;
        GameObject newPoint = spawn.SpawnPoint(components.PreFinish, VectorDeterminant.transform.position);
        routeForEnemies.SetEndPoint(newPoint.transform);
        routeForEnemies.SearchPath();
        VectorDeterminant.SetActive(false);
        cellController.ChangeCellTag("tower", "CellForTower");
        cellController.ArrangingDecor();
        gameController.StartGame();
    }

    private void RefreshCells()
    {
        DirectionOfVerificationNorth = directions.North.GetComponent<GettingCellReference>().CellPosition;
        DirectionOfVerificationSouth = directions.South.GetComponent<GettingCellReference>().CellPosition;
        DirectionOfVerificationEast = directions.East.GetComponent<GettingCellReference>().CellPosition;
    }

    private void AddTerritoryForTowers()
    {
        if (DirectionOfVerificationNorth.GetComponent<Cell>().VarietyCell == "standart" && DetermineWhetherPossibleChangeTypeCell()) ChangeCell(DirectionOfVerificationNorth, components.ZoneForTower, "tower");
        else if (DirectionOfVerificationNorth.GetComponent<Cell>().VarietyCell == "standart") ChangeCell(DirectionOfVerificationNorth, null, "none");
        if (DirectionOfVerificationSouth.GetComponent<Cell>().VarietyCell == "standart" && DetermineWhetherPossibleChangeTypeCell()) ChangeCell(DirectionOfVerificationSouth, components.ZoneForTower, "tower");
        else if (DirectionOfVerificationSouth.GetComponent<Cell>().VarietyCell == "standart") ChangeCell(DirectionOfVerificationSouth, null, "none");
        if (DirectionOfVerificationEast.GetComponent<Cell>().VarietyCell == "standart" && DetermineWhetherPossibleChangeTypeCell()) ChangeCell(DirectionOfVerificationEast, components.ZoneForTower, "tower");
        else if (DirectionOfVerificationEast.GetComponent<Cell>().VarietyCell == "standart") ChangeCell(DirectionOfVerificationEast, null, "none");
    }

    private bool DetermineWhetherPossibleChangeTypeCell()
    {
        int randomNumber;
        randomNumber = Random.Range(0, NumberForMaximumRandomNumber);
        if (randomNumber != 0 && randomNumber != 4 && randomNumber != 6) return true;
        else return false;
    }

    private void ChangeCell(GameObject cellReferenceReceived, Sprite newSprite, string assignedView)
    {
        GameObject cell = cellReferenceReceived;
        if (newSprite) cell.GetComponent<SpriteRenderer>().sprite = newSprite;
        if (cell.GetComponent<Cell>().VarietyCell != "trail") cell.GetComponent<Cell>().VarietyCell = assignedView;
        if (cell.GetComponent<Cell>().VarietyCell == "trail") cell.layer = 8;
    }
}