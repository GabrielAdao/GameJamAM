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
    //public Image EnergyBar;

    public bool isRecharging = false;

    public GameObject panel;
    private bool isInPlayZone = false;
    private void Start() {
        currentEnergy = maxEnergy;
        if(panel != null){
            panel.SetActive(false);
        }
        //UpdateEnergyBar();
    }

    private void Update() { 
         if (isInPlayZone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                panel.SetActive(true);
                isRecharging = true;
                StopAllCoroutines();
                StartCoroutine(RechargeEnergy());
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                panel.SetActive(false);
                isRecharging = false;
                StopAllCoroutines();
            }
        }

        if(currentEnergy < 1){
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            playerMovement.Stunned(5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayZone")){
            isInPlayZone = true;
        } 
    }

    public void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("WorkZone")){
            WorkZone workZone = other.GetComponent<WorkZone>();
            if(workZone != null && !workZone.IsOnCooldown()){
                workZone.HandleWorkZone(this);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("PlayZone")){
            isInPlayZone = false;
            isRecharging = false;
            if(panel != null){
                panel.SetActive(false);
            }
        }
    }

    IEnumerator RechargeEnergy(){
        while(isRecharging && currentEnergy < maxEnergy){
            currentEnergy += energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy,0, maxEnergy);
            yield return null;
        }
    }

    public void SpendEnergy(float amount){
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

    }

    //void UpdateEnergyBar(){
    //    if(EnergyBar != null){
    //        EnergyBar.fillAmount = currentEnergy / maxEnergy;
    //    }
   // }
}
