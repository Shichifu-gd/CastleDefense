using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ListEnemy")]
public class ScriptableObjectsLevelListEnemy : ScriptableObject
{
    public AnimationCurve Distribution;
    public ScriptableObjectsEnemy[] EnemyList;
}