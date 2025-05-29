using UnityEngine;

public class CatapultRotator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private SpriteRenderer catapultSR;

    private Transform target;

    private void Update() {
        if (target == null) {
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized; 
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            catapultSR.sprite = spriteRight;
            catapultSR.flipX = direction.x < 0;
        } else {
            if (direction.y > 0)
                catapultSR.sprite = spriteUp;
            else
                catapultSR.sprite = spriteDown;
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }
}
