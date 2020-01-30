using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tower")]
public class ScriptableObjectsTower : ScriptableObject
{
    public ScriptableObjectsProjectile ScrObjProjectile;
    public Sprite SpriteTower;
    public Sprite SpriteForIco;
    public Color ColorForTest;
    public float AttackRangeTower;
    public float BasicDelayAttackTower;
    public int PriceForUpgradeTower;
    public int PriceTower;
    public ScriptableObjectsTower[] Levels;
}