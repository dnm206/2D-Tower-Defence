using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int hitPoints = 5;
    [SerializeField] private int goldWorth = 20;

    private bool isDead = false; // Thêm biến này để chặn việc chết 2 lần (do trúng nhiều đạn cùng lúc)

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Nếu đã chết rồi thì không nhận sát thương nữa

        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true; // Đánh dấu đã chết

        // 1. Cộng tiền và hiện chữ bay (Chỉ gọi DUY NHẤT ở đây)
        if (BuildManager.main != null)
        {
            BuildManager.main.AddCurrency(goldWorth, transform.position);
        }

        // 2. Báo cho Spawner để quản lý số lượng quái
        if (EnemySpawner.onEnemyDestroyed != null)
        {
            EnemySpawner.onEnemyDestroyed.Invoke();
        }

        // 3. Xóa con quái khỏi game
        Destroy(gameObject);
    }
}