using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Enemy targetEnemy;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform projectileSpawnPosition;

    private bool readyToAttack = true;
    private ActionOnTimer attackDelayTimer;


    private void Awake()
    {
        attackDelayTimer = gameObject.AddComponent<ActionOnTimer>();


    }


    private void Update()
    {
        if (!readyToAttack) return;
        LookForTargets();
        if (targetEnemy == null) return;
        Attack();

        
    }


    private void SetAttackDelayTimer()
    {
        attackDelayTimer.SetTimer(attackSpeed, () => { readyToAttack = true; }, false);
    }
    

    private void Attack()
    {
        ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
        readyToAttack = false;
        SetAttackDelayTimer();
    }


    private void LookForTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider2D in colliders)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy == null) continue;



            if (targetEnemy == null) targetEnemy = enemy;
            else
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) > Vector3.Distance(transform.position, targetEnemy.transform.position)) continue;
                targetEnemy = enemy;
            }


        }
    }
}
