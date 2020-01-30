using UnityEngine;

public class GettingCellReference : MonoBehaviour
{
    public GameObject CellPosition { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Cell>()) CellPosition = collision.gameObject;
    }
}