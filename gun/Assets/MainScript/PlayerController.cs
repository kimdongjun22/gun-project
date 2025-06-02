using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 5f;
    public float skillCooldown = 5f;
    private float lastSkillTime = -10f;

    void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0))
            Fire();

        if (Input.GetKeyDown(KeyCode.Space))
            UseSkill();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, v, 0) * moveSpeed * Time.deltaTime);
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void UseSkill()
    {
        if (Time.time - lastSkillTime >= skillCooldown)
        {
            Skill.Activate(transform.position);
            lastSkillTime = Time.time;
        }
    }
}
