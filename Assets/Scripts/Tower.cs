using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public event EventHandler<OnTargetSetEventEventArgs> OnTargetSetEvent;


    public class OnTargetSetEventEventArgs : EventArgs
    {
        public Enemy targetEnemy;
    }


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
        LookForClosesTarget();
        if (targetEnemy == null) return;
        Attack();

        
    }

    private void Attack()
    {
        
        readyToAttack = false;
    
        attackDelayTimer.SetTimer(attackSpeed, () => 
        {
            ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
            readyToAttack = true; 

        }, false);
    }


    private void LookForClosesTarget()
    {
        Enemy enemy = EnemyManager.Instance.GetClosestEnemy(transform.position);
        if (enemy != null)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < attackRange)
            {
                targetEnemy = enemy;
            }
        }
        OnTargetSetEvent?.Invoke(this, new OnTargetSetEventEventArgs { targetEnemy = targetEnemy });
    }
    public ActionOnTimer GetAttackTimer()
    {
        return attackDelayTimer;
    }



    //old one not in use
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
