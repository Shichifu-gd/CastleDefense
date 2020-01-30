using UnityEngine;

public class DeterminantVectorCurrentPosition : MonoBehaviour
{
    public GameObject CurrentCell { get; set; }
    public GameObject North;
    public GameObject South;
    public GameObject East;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Cell>()) CurrentCell = collision.gameObject;
    }
}