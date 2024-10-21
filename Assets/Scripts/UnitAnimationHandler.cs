using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Tower tower;
    [Header("Settings")]
    [SerializeField] private float animationAttackTime;
    [SerializeField] private float offSetYSwitchToBackAnimation = 0.1f;

    private Enemy target;
    private ActionOnTimer attackTimer;
    

    
    private void Start()
    {
        tower.OnTargetSetEvent += Tower_OnTargetSetEvent;
        attackTimer = tower.GetAttackTimer();
    }
    private void OnDestroy()
    {
        tower.OnTargetSetEvent -= Tower_OnTargetSetEvent;
    }

    private void Update()
    {
        if (target == null)
        {
            animator.SetFloat("f_moveDirectionX", 0);
            animator.SetFloat("f_moveDirectionY", 0);
            return;
        }
        Vector3 lookDirection = (target.transform.position - transform.position).normalized;

        if(lookDirection.y > 0+offSetYSwitchToBackAnimation)
        {
            animator.SetFloat("f_moveDirectionX", lookDirection.x);
            animator.SetFloat("f_moveDirectionY", 1);
        }
        else
        {
            animator.SetFloat("f_moveDirectionX", lookDirection.x);
            animator.SetFloat("f_moveDirectionY", lookDirection.y);
        }

        if(attackTimer.GetCurrentTime() <= animationAttackTime)
        {
            animator.SetTrigger("Attack");
            
        }

        
        

    }


    private void Tower_OnTargetSetEvent(object sender, Tower.OnTargetSetEventEventArgs e)
    {
        target = e.targetEnemy;
        
    }







}
