using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip deathClip;

    [Header("Attributes")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyHealth;
    [SerializeField] private int worth;

    private Vector3 targetPosition;
    private int pointIndex = 1;
    private bool isDestroyed = false;
    private int flipX = 1;

    private void Start() {
        enemySpeed *= Random.Range(1f, 1.2f);
        targetPosition = LevelManager.main.GetPath()[pointIndex].position;

        targetPosition += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
        targetPosition.x *= flipX;
    }

    private void Update() {
        if (Vector2.Distance(targetPosition, transform.position) <= 0.1f) {
            pointIndex++;

            if (pointIndex == LevelManager.main.GetPath().Length) {
                LevelManager.main.DecreaseCastleHealth();
                EnemyManager.main.DestroyEnemy();
                Destroy(gameObject);
                return;
            } else {
                targetPosition = LevelManager.main.GetPath()[pointIndex].position;
                targetPosition += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
                targetPosition.x *= flipX;
            }
        }
    }

    private void FixedUpdate() {
        if (!isDestroyed) {
            Vector2 direction = (targetPosition - transform.position).normalized;
            rb.linearVelocity = direction * enemySpeed;

            UpdateAnimationDirection(direction);
        }
    }    

    public void TakeDamage(float damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0 && !isDestroyed) {
            DeathBeforeAnim();
        }
    }

    private void DeathBeforeAnim() {
        isDestroyed = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        BuildManager.main.IncreaseCurrency(worth);
        LevelManager.main.IncreaseEnemiesKilled();
        EnemyManager.main.DestroyEnemy();
        SFXManager.main.PlaySFX(deathClip);
       
        anim.SetBool("isDead", true);
    }

    public void DeathAfterAnim() {
        Destroy(gameObject);
    }

    private void UpdateAnimationDirection(Vector2 dir) {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
            GetComponent<SpriteRenderer>().flipX = dir.x > 0;
            anim.Play("WalkSide");
        } else {
            GetComponent<SpriteRenderer>().flipX = false;
            if (dir.y > 0) {
                anim.Play("WalkUp");
            } else {
                anim.Play("WalkDown");
            }
        }

    }

    public void SetFlipX(int flipX) {
        this.flipX = flipX;
    }

    public float GetEnemySpeed() {
        return enemySpeed;
    }

    public float GetEnemyHealth() {
        return enemyHealth;
    }
}
