using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public float maxEnergy = 5f;
    public float currentEnergy;
    public float energyRechargeRate = 1f;
    public bool isRecharging = false;

    public GameObject GGGpanel;
    private bool isInPlayZone = false;

    public Sprite energySprite; 
    public GameObject energyPanel; 
    private List<Image> energyImages = new List<Image>();

    private void Start() {
        currentEnergy = maxEnergy;
        if (GGGpanel != null) {
            GGGpanel.SetActive(false);
        }
        UpdateEnergyImages();
    }

    private void Update() { 
        if (isInPlayZone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GGGpanel.SetActive(true);
                isRecharging = true;
                StopAllCoroutines();
                StartCoroutine(RechargeEnergy());
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                GGGpanel.SetActive(false);
                isRecharging = false;
                StopAllCoroutines();
            }
        }

        if (currentEnergy < 1) {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            playerMovement.Stunned(5f);
        }

        UpdateEnergyImages(); 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayZone")) {
            isInPlayZone = true;
        } 
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("WorkZone")) {
            WorkZone workZone = other.GetComponent<WorkZone>();
            if (workZone != null && !workZone.IsOnCooldown()) {
                workZone.HandleWorkZone(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayZone")) {
            isInPlayZone = false;
            isRecharging = false;
            if (GGGpanel != null) {
                GGGpanel.SetActive(false);
            }
        }
    }

    IEnumerator RechargeEnergy() {
        while (isRecharging && currentEnergy < maxEnergy) {
            currentEnergy += energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            yield return null;
        }
    }

    public void SpendEnergy(float amount) {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    void UpdateEnergyImages() {
        while (energyImages.Count > currentEnergy) {
            Destroy(energyImages[energyImages.Count - 1].gameObject);
            energyImages.RemoveAt(energyImages.Count - 1);
        }

        while (energyImages.Count < currentEnergy) {
            GameObject newImage = new GameObject("EnergyImage");
            newImage.transform.SetParent(energyPanel.transform);
            Image img = newImage.AddComponent<Image>();
            img.sprite = energySprite;

            RectTransform rectTransform = img.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            
            energyImages.Add(img);


        }
    }
}