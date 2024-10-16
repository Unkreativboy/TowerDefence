using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;


    private Enemy targetEnemy;

    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrow = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrow, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
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
            Destroy(gameObject);
        }


    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
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
