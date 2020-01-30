using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;
    private EnemyController enemyController;

    [SerializeField]
    private SpriteRenderer SpriteEnemy = new SpriteRenderer();
    private Collider2D EnemyCollider2D;
    public Image ImageCurrentHealth;

    private int DirectionPointIndex = 0;
    private int MaxHealthEnemy;
    private int HealthEnemy;
    private int RewardGold;
    private int RewardScore;
    private int EnemyDamage;

    private float SpeedMove;
    private float BaseSpeedMove;
    private float DebuffTime;
    private float PowerDebuff;

    public bool IsDead { get; set; } = false;

    private List<Vector2> DirectionPoints = new List<Vector2>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        EnemyCollider2D = GetComponent<Collider2D>();
    }

    public void AssignValuesForEnemy(ScriptableObjectsEnemy scrObjEnemy, List<Vector2> way)
    {
        DirectionPoints = way;
        EnemyDamage = scrObjEnemy.DamageEnemy;
        DirectionPointIndex = DirectionPoints.Count - 1;
        enemyController.RegisterEnemy(gameObject);
        SpriteEnemy.sprite = scrObjEnemy.SpriteEnemy;
        SpriteEnemy.color = scrObjEnemy.SpriteColor;
        HealthEnemy = scrObjEnemy.HealthEnemy;
        MaxHealthEnemy = HealthEnemy;
        SpeedMove = scrObjEnemy.SpeedMoveEnemy;
        BaseSpeedMove = SpeedMove;
        RewardGold = Random.Range(scrObjEnemy.MinReward, scrObjEnemy.MaxReward);
        RewardScore = Random.Range(scrObjEnemy.MinScore, scrObjEnemy.MaxScore);
    }

    private void FixedUpdate()
    {
        if (IsDead == false) GoToPoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>())
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            ProjectileType projectileType = projectile.ProjectileTypeEnum;
            if (projectile)
            {
                if (projectileType == ProjectileType.FireFist)
                {
                    PowerDebuff = projectile.EffectImpactProjectile;
                    DebuffTime = projectile.EffectTime;
                    TakingDebuff();
                }
                TakingDamage(projectile.AttackDamage);
                Destroy(collision.gameObject);
            }
        }
    }

    private void GoToPoint()
    {
        Vector2 curentPosition = transform.position;
        Vector2 vector = DirectionPoints[DirectionPointIndex] - curentPosition;
        transform.Translate(vector.normalized * Time.deltaTime * SpeedMove);
        if (Vector2.Distance(transform.position, DirectionPoints[DirectionPointIndex]) < .02f)
        {
            if (DirectionPointIndex > 0) DirectionPointIndex--;
            else EnemyHasReachedFinish();
        }
    }

    private void EnemyHasReachedFinish()
    {
        int damage = -Mathf.Abs(EnemyDamage);
        player.GetLife(damage);
        EnemyIsDead();
    }

    public void TakingDamage(int damage)
    {
        if ((HealthEnemy - damage) > 0)
        {
            HealthEnemy -= damage;
            float num = HealthEnemy;
            ImageCurrentHealth.fillAmount = num / MaxHealthEnemy;
        }
        else
        {
            player.SetGold(RewardGold);
            enemyController.SetNumberOfDead();
            EnemyIsDead();
        }
    }

    public void TakingDebuff()
    {
        SpeedMove = BaseSpeedMove;
        StopCoroutine("slow");
        StartCoroutine(DebuffSpeedReduction());
    }

    IEnumerator DebuffSpeedReduction()
    {
        if (SpeedMove > 0.5f) SpeedMove -= PowerDebuff;
        else SpeedMove = 0.5f;
        yield return new WaitForSeconds(DebuffTime);
        SpeedMove = BaseSpeedMove;
    }

    private void EnemyIsDead()
    {
        IsDead = true;
        EnemyCollider2D.enabled = false;
        enemyController.UnRegisterEnemy(gameObject, RewardScore);
    }
}