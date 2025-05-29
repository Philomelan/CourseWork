using UnityEngine;

public class TileBehaviour : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private Color startColor;  

    private void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        if (!OverlayManager.main.GetOverlayOpened()) {
            sr.color = hoverColor;
        }
    }

    private void OnMouseExit() {
        if (!OverlayManager.main.GetOverlayOpened()) {
            sr.color = startColor;
        }
    }

    private void OnMouseDown() {
        if (OverlayManager.main.GetOverlayOpened()) {
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (BuildManager.main.DecreaseCurrency(towerToBuild.cost)) {
            LevelManager.main.IncreaseGoldSpent(towerToBuild.cost);

            GameObject tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            tower.transform.localScale *= LevelManager.main.GetScaleCoef();
            tower.GetComponent<TowerBehaviour>().SetBuildTile(gameObject);
            gameObject.SetActive(false);            
        }                   
    }
}
