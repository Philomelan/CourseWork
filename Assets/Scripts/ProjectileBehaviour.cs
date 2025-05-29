using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioClip deathClip;

    private Vector3 towerPosition;
    private Transform target;    
    private float towerRange;
    private float projectileSpeed;
    private float projectileDamage;

    private void Start() {
        towerPosition = transform.position;
    }

    public void SetInfo(Transform target, float towerRange, float projectileSpeed, float projectileDamage) {
        this.target = target;
        this.towerRange = towerRange;
        this.projectileSpeed = projectileSpeed;
        this.projectileDamage = projectileDamage;
    }

    private void FixedUpdate() {
        if ((Vector2.Distance(transform.position, towerPosition) > towerRange) || (!target)) {
            SFXManager.main.PlaySFX(deathClip);
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * projectileSpeed;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(projectileDamage);
        SFXManager.main.PlaySFX(deathClip);
        Destroy(gameObject);
    }
}
