using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int hitPoints = 5;
    [SerializeField] private int goldWorth = 20; // Số tiền quái này trả cho người chơi khi chết

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            // Cộng tiền vào BuildManager trước khi biến mất
            BuildManager.main.AddCurrency(goldWorth);

            // Báo cho Spawner
            if (EnemySpawner.onEnemyDestroyed != null)
                EnemySpawner.onEnemyDestroyed.Invoke();

            Destroy(gameObject);
        }
    }
}