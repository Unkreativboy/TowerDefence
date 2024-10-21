using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;


    private Enemy targetEnemy;
    private Vector3 targetEnemyPosition;


    public static ArrowProjectile Create(Vector3 position, Enemy enemy, Vector3 enemyPosition)
    {
        Transform pfArrow = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrow, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy, enemyPosition);
        return arrowProjectile;
    }



    private void Update()
    {
        if (targetEnemy != null)
        {

            Vector3 moveDir = (targetEnemy.transform.position - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
        }
        else
        {

            Vector3 moveDir = (targetEnemyPosition - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
            Destroy(gameObject,2f);
        }


    }

    private void SetTarget(Enemy targetEnemy, Vector3 enemyPosition)
    {
        this.targetEnemy = targetEnemy;
        targetEnemyPosition = enemyPosition;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        damageable.Damage(damage);
        Destroy(gameObject);
    }

}
