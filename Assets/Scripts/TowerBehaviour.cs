using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TowerBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject rangeIndicator;

    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip buildClip;
    [SerializeField] private AudioClip destroyClip;

    [Header("Attributes")]
    [SerializeField] private float towerRange;
    [SerializeField] private float towerSpeed;
    [SerializeField] private int towerType;
    [SerializeField] private int nextTowerIndex;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileRange;
    [SerializeField] private bool isArea = false;

    private Transform target;
    private float timeUntilFire;
    private Color startColor; 
    private GameObject BuildTile;
    private bool justOpenedUI;
    private float towerTypeCoef;

    private void Start() {
        startColor = sr.color;
        SFXManager.main.PlaySFX(buildClip);

        if (towerType == 1) {
            towerTypeCoef = 0.3f;
        } else if (towerType == 2) {
            towerTypeCoef = 1.3f;
        } else {
            towerTypeCoef = 1.16f;
        }
        float diameter = towerRange * towerTypeCoef;
        rangeIndicator.transform.localScale = new Vector3(diameter, diameter, 1f);
        rangeIndicator.SetActive(false);
    }

    private void Update() { 
        if (upgradeUI.activeSelf && Input.GetMouseButtonDown(0)) {
            if (!EventSystem.current.IsPointerOverGameObject() && !justOpenedUI) {
                upgradeUI.SetActive(false);
            }
        }

        if (justOpenedUI) {
            justOpenedUI = false;
        }

        if (target == null) {
            FindTarget();
            return;
        }
        
        if (!CheckTargetIsInRange()) {
            target = null;
            return;
        }

        CatapultRotator catapultRotator = GetComponent<CatapultRotator>();
        if (catapultRotator != null) {
            catapultRotator.SetTarget(target);
        }

        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / towerSpeed) {
            Shoot();
            timeUntilFire = 0f;
        }
    }

    private void FindTarget() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerRange, enemyMask);
        if (hits.Length == 0) {
            return;
        }

        if (towerType == 1) {
            float enemySpeed, maxSpeed = 0f;
            int maxSpeedIndex = 0;
            for (int i = 0; i < hits.Length; i++) {
                enemySpeed = hits[i].GetComponent<EnemyBehaviour>().GetEnemySpeed();
                if (enemySpeed > maxSpeed) {
                    maxSpeed = enemySpeed;
                    maxSpeedIndex = i;
                }
            }
            target = hits[maxSpeedIndex].transform;
        } else if (towerType == 2) {
            target = hits[0].transform;
        } else {
            float enemyHealth, maxHealth = 0f;
            int maxHealthIndex = 0;
            for (int i = 0; i < hits.Length; i++) {
                enemyHealth = hits[i].GetComponent<EnemyBehaviour>().GetEnemyHealth();
                if (enemyHealth > maxHealth) {
                    maxHealth = enemyHealth;
                    maxHealthIndex = i;
                }
            }
            target = hits[maxHealthIndex].transform;
        }        
    }

    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= towerRange;
    }

    private void Shoot() {
        SFXManager.main.PlaySFX(shootClip);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.localScale *= LevelManager.main.GetScaleCoef();

        if (!isArea) {
            ProjectileBehaviour projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
            projectileBehaviour.SetInfo(target, towerRange, projectileSpeed, projectileDamage);
        } else {
            AreaProjectileBehaviour areaProjectileBehaviour = projectile.GetComponent<AreaProjectileBehaviour>();
            areaProjectileBehaviour.SetInfo(target, enemyMask, towerRange, projectileSpeed, projectileDamage, projectileRange);
        }
        
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, towerRange);
    }
    #endif

    private void OnMouseEnter() {
        if (!OverlayManager.main.GetOverlayOpened()) {
            sr.color = hoverColor;
            rangeIndicator.SetActive(true);
        }       
    }

    private void OnMouseExit() {
        if (!OverlayManager.main.GetOverlayOpened()) {
            sr.color = startColor;
            rangeIndicator.SetActive(false);
        }
    }

    private void OnMouseDown() {
        if (!OverlayManager.main.GetOverlayOpened()) {
            upgradeUI.SetActive(true);
            justOpenedUI = true;
        }
    }
    
    public void UpgradeTower()
    {
        Tower towerToBuild = BuildManager.main.GetTower(nextTowerIndex);
        if (BuildManager.main.DecreaseCurrency(towerToBuild.cost)) {
            LevelManager.main.IncreaseGoldSpent(towerToBuild.cost);
            SFXManager.main.PlaySFX(buildClip);

            GameObject tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            tower.transform.localScale *= LevelManager.main.GetScaleCoef();
            tower.GetComponent<TowerBehaviour>().SetBuildTile(BuildTile);
            Destroy(gameObject);
        } else {
            upgradeUI.SetActive(false);
        }
    }

    public void DestroyTower() {
        SFXManager.main.PlaySFX(destroyClip);
        BuildTile.SetActive(true);
        Destroy(gameObject);
    }

    public void SetBuildTile(GameObject BuildTile) {
        this.BuildTile = BuildTile;
    }
}
