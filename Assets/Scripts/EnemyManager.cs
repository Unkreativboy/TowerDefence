using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Singelton
    public static EnemyManager Instance;

    public List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public Enemy GetClosestEnemy(Vector2 playerPosition)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        if (enemies.Count == 0) return null;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(playerPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

}
