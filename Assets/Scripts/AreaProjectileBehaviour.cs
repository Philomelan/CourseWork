using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AreaProjectileBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip deathClip;

    private Vector3 towerPosition;
    private Transform target;
    private LayerMask enemyMask;
    private float towerRange;
    private float projectileSpeed;
    private float projectileDamage;
    private float projectileRange;

    private void Start() {
        towerPosition = transform.position;
    }

    public void SetInfo(Transform target, LayerMask enemyMask, float towerRange, float projectileSpeed, float projectileDamage, float projectileRange) {
        this.target = target;
        this.towerRange = towerRange;
        this.enemyMask = enemyMask;
        this.projectileSpeed = projectileSpeed;
        this.projectileDamage = projectileDamage;
        this.projectileRange = projectileRange;
    }

    private void FixedUpdate() {
        if ((Vector2.Distance(transform.position, towerPosition) > towerRange) || (!target)) {
            if (rb.simulated) {
                ExplodeBeforeAnim();                
            }
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * projectileSpeed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {        
        ExplodeBeforeAnim();
    }

    private void ExplodeBeforeAnim() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, projectileRange, enemyMask);
        foreach (Collider2D hit in hits) {
            hit.GetComponent<EnemyBehaviour>().TakeDamage(projectileDamage);
        }

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        SFXManager.main.PlaySFX(deathClip);

        anim.SetBool("isExploded", true);
    }

    public void ExplodeAfterAnim() {
        Destroy(gameObject);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, projectileRange);
    }
    #endif
}
