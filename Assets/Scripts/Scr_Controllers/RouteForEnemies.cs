using System.Collections.Generic;
using UnityEngine;

public class RouteForEnemies : MonoBehaviour
{
    private PathFinder pathFinder;
    private Transform EndPoint;
    public List<Vector2> NodesList;

    public void SetStartPoint(PathFinder point)
    {
        pathFinder = point;
    }

    public void SetEndPoint(Transform point)
    {
        EndPoint = point;
    }

    public void SearchPath()
    {
        NodesList = pathFinder.GetPath(EndPoint);
    }
}