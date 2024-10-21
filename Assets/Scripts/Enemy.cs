using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float targetMaxRadius;

    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private HealthSystem healthSystem;
    private float moveSpeed = 6f;


    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, position,Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        EnemyManager.Instance.RegisterEnemy(enemy);
        return enemy;
    }


    private void Start()
    {   
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        Building.OnBuildingDestroyed += Building_OnBuildingDestroyed;
        Building.OnBuildingBuild += Building_OnBuildingBuild;

        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        rigidbody2d = GetComponent<Rigidbody2D>();
        LookForTargets();
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        healthSystem.OnDied -= HealthSystem_OnDied;
        Building.OnBuildingDestroyed -= Building_OnBuildingDestroyed;
        Building.OnBuildingBuild -= Building_OnBuildingBuild;
        EnemyManager.Instance.UnregisterEnemy(this);
    }

    private void Building_OnBuildingBuild(object sender, System.EventArgs e)
    {
        LookForTargets();
    }

    private void Building_OnBuildingDestroyed(object sender, System.EventArgs e)
    {
        LookForTargets();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (targetTransform == null)
        {
            LookForTargets();
            return;
        }

        Vector3 moveDir = (targetTransform.position - transform.position).normalized;
        rigidbody2d.velocity = moveDir * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            IDamageable damageable = building.GetComponent<IDamageable>();
            damageable.Damage(10);
            Destroy(gameObject);
        }
    }

    private void LookForTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach (Collider2D collider2D in colliders)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building == null) continue;
            if (targetTransform == null) targetTransform = building.transform;
            else
            {
                if (Vector3.Distance(transform.position, building.transform.position) > Vector3.Distance(transform.position, targetTransform.position)) continue;
                targetTransform = building.transform;
            }


        }
    }


}
