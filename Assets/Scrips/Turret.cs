using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f; // Số lượt bắn mỗi giây
    [SerializeField] private float rotateSpeed = 500f;
    [SerializeField] private int bulletsPerShot = 1; // <--- SỐ VIÊN ĐẠN MỖI LẦN BẮN

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                StartCoroutine(ShootRoutine()); // Dùng Coroutine để đạn bay ra cách nhau một chút cho đẹp
                timeUntilFire = 0f;
            }
        }
    }

    // Hàm xử lý bắn nhiều viên đạn
    private IEnumerator ShootRoutine()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Shoot();
            // Đợi 0.1 giây giữa mỗi viên đạn để thấy rõ tank bắn 2-3 viên
            if (bulletsPerShot > 1) yield return new WaitForSeconds(0.1f);
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    // ... (Giữ nguyên các hàm FindTarget, RotateTowardsTarget, CheckTargetIsInRange như cũ)
    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= targetingRange) target = nearestEnemy.transform;
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}