using UnityEngine.EventSystems;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string VarietyCell { get; set; } = "standart";
    public bool EndOfRoad;
    public LayerMask layer;
    public Color MainColor;
    public Color AdditionalColor;

    private void OnMouseEnter()
    {
        if (VarietyCell == "tower" && OnMouse()) GetComponent<SpriteRenderer>().color = AdditionalColor;
    }

    public bool OnMouse()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) return true;
        else return false;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = MainColor;
    }
}