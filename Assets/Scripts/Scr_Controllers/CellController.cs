using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private SpritesDecor spritesDecor;
    public Spawn spawn;
    public GameObject PreDecor;
    public List<GameObject> ListCell;

    public void Awake()
    {
        spritesDecor = GetComponent<SpritesDecor>();
    }

    public void ChangeCellTag(string varietyCell, string newTag)
    {
        for (int index = 0; index < ListCell.Count; index++)
        {
            if (ListCell[index].GetComponent<Cell>().VarietyCell == varietyCell) ListCell[index].tag = newTag;
        }
    }

    #region For spawn decor
    public void ArrangingDecor()
    {
        int random;
        for (int index = 0; index < ListCell.Count; index++)
        {
            if (ListCell[index].GetComponent<Cell>().VarietyCell == "standart")
            {
                random = Random.Range(0, 11);
                if (random == 5 || random == 8 || random == 2) ChoiceDecor(index);
            }
        }
    }

    public void ChoiceDecor(int index)
    {
        int randomVarieties = Random.Range(0, 5);
        int randomDecor;
        if (randomVarieties == 0)
        {
            randomDecor = Random.Range(0, spritesDecor.DecorStone.Length);
            NewDecor(spritesDecor.DecorStone[randomDecor], index);
        }
        if (randomVarieties == 1)
        {
            randomDecor = Random.Range(0, spritesDecor.DecorTree.Length);
            NewDecor(spritesDecor.DecorTree[randomDecor], index);
        }
        if (randomVarieties == 2 || randomVarieties == 3 || randomVarieties == 4)
        {
            randomDecor = Random.Range(0, spritesDecor.DecorGrass.Length);
            NewDecor(spritesDecor.DecorGrass[randomDecor], index);
        }
    }

    private void NewDecor(Sprite sprite, int index)
    {
        GameObject newDecor = spawn.SpawnDecor(PreDecor, ListCell[index].transform.position);
        newDecor.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    #endregion
}