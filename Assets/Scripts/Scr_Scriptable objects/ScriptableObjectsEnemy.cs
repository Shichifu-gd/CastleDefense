﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Characters/Enemy")]
public class ScriptableObjectsEnemy : ScriptableObject
{
    public Sprite SpriteEnemy;
    public Color SpriteColor;
    public float SpeedMoveEnemy;
    public int HealthEnemy;
    public int DamageEnemy;
    public int MinReward;
    public int MaxReward;
    public int MinScore;
    public int MaxScore;
}