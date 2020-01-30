using UnityEngine;

public class Tower : MonoBehaviour
{
    private ScriptableObjectsProjectile ScrObjProjectile;
    private EnemyController enemyController;
    public GameController gameController;
    private Enemy Target;
    private Spawn spawn;

    public int SalePrice { get; set; }
    public int CurrentLevelTower { get; set; }
    public int PriceForUpgrade { get; set; }
    public int TowerLevel { get; set; }

    private float BasicDelayAttack;
    private float CurrentDelayAttack;
    private float AttackRange;

    private bool SwitchButton;

    public GameObject ThisTower;
    public GameObject PreProjectile;

    public SpriteRenderer TowerSprite;

    public ScriptableObjectsTower[] TowerLevels;

    private void Awake()
    {
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        spawn = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawn>();
    }

    public bool CanIncrease()
    {
        if (TowerLevel < TowerLevels.Length) return true;
        return false;
    }

    public void UpLevelTower()
    {
        if (TowerLevel < TowerLevels.Length)
        {
            TowerComponents(TowerLevels[TowerLevel]);
            TowerLevel++;
        }
    }

    public void TowerComponents(ScriptableObjectsTower tower)
    {
        TowerSprite.sprite = tower.SpriteTower;
        TowerSprite.color = tower.ColorForTest;
        SalePrice += (tower.PriceTower + tower.PriceForUpgradeTower) / 3;
        PriceForUpgrade = tower.PriceForUpgradeTower;
        AttackRange = tower.AttackRangeTower;
        BasicDelayAttack = tower.BasicDelayAttackTower;
        CurrentDelayAttack = BasicDelayAttack;
        ScrObjProjectile = tower.ScrObjProjectile;
    }

    public void SetTowerLevel(ScriptableObjectsTower[] towerLevels)
    {
        TowerLevels = towerLevels;
    }

    private void Update()
    {
        if (CanAttack()) GetTarget();
        if (CurrentDelayAttack > 0) CurrentDelayAttack -= Time.deltaTime;
    }

    private bool CanAttack()
    {
        if (CurrentDelayAttack <= 0) return true;
        return false;
    }

    private void GetTarget()
    {
        bool pause = gameController.OnPause();
        if (!pause)
        {
            Transform target = null;
            float enemyDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemyController.EnemyList)
            {
                float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
                if (currentDistance < enemyDistance && currentDistance <= AttackRange)
                {
                    target = enemy.transform;
                    enemyDistance = currentDistance;
                }
            }
            if (target != null) AttackTower(target);
        }
    }

    private void AttackTower(Transform thisEnemy)
    {
        CurrentDelayAttack = BasicDelayAttack;
        GameObject newProjectile = spawn.SpawnProjectile(PreProjectile, gameObject.transform.position);
        newProjectile.GetComponent<Projectile>().AssignValues(thisEnemy, ScrObjProjectile);
    }
}