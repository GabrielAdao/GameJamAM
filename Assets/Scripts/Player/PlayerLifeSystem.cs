using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLifeSystem : MonoBehaviour
{
    public int playerMaxLife = 5;
    public int playerCurrentLife;

    public Sprite lifeSprite; 
    public Transform lifePanel; 
    private List<Image> lifeImages = new List<Image>();

    private void Start() {
        playerCurrentLife = playerMaxLife;
        CreateLifeImages();
    }

    public void PlayerLoseLife() {
        if (playerCurrentLife > 0) {
            playerCurrentLife -= 1;
            RemoveLifeImage();
        }
        
        if (playerCurrentLife <= 0) {
            PlayerDeath();
        }
    }

    void PlayerDeath() {
        SceneManager.LoadScene("Gameover");
    }

    void CreateLifeImages() {
        for (int i = 0; i < playerMaxLife; i++) {
            GameObject lifeImageObject = new GameObject("LifeImage");
            lifeImageObject.transform.SetParent(lifePanel);

            Image lifeImage = lifeImageObject.AddComponent<Image>();
            lifeImage.sprite = lifeSprite;

            RectTransform rectTransform = lifeImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50); 
            lifeImages.Add(lifeImage);
        }
    }

    void RemoveLifeImage() {
        if (lifeImages.Count > 0) {
            Image lastLifeImage = lifeImages[lifeImages.Count - 1];
            lifeImages.Remove(lastLifeImage);
            Destroy(lastLifeImage.gameObject);
        }
    }
}