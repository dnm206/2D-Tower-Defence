using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        // Lấy điểm đến đầu tiên từ mảng path của LevelManager
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        // Kiểm tra nếu đã đến gần điểm mục tiêu (khoảng cách <= 0.1)
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++; // Tăng chỉ số để lấy điểm tiếp theo

            // Nếu đi hết danh sách các điểm (về đích)
            if (pathIndex == LevelManager.main.path.Length)
            {
                // QUAN TRỌNG: Báo cho Spawner biết quái này đã biến mất
                EnemySpawner.onEnemyDestroyed.Invoke();

                Destroy(gameObject); // Xóa quái khi về đích
                return;
            }
            else
            {
                // Cập nhật mục tiêu là điểm tiếp theo trong mảng
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // Tính toán hướng từ vị trí hiện tại đến mục tiêu
        Vector2 direction = (target.position - transform.position).normalized;

        // Gán vận tốc cho Rigidbody2D (Dùng cho Unity 6)
        rb.linearVelocity = direction * moveSpeed;
    }
}