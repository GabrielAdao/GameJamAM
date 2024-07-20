using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public float maxEnergy = 5f;
    public float currentEnergy;
    public float energyRechargeRate = 1f;
    //public Image EnergyBar;

    private bool isRecharging = false;

    private void Start() {
        currentEnergy = maxEnergy;
        //UpdateEnergyBar();
    }

    private void Update() {
        //UpdateEnergyBar();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayZone")){
            isRecharging = true;
            StopAllCoroutines();
            StartCoroutine(RechargeEnergy());
        } else if(other.CompareTag("WorkZone")){
            WorkZone workzone = other.GetComponent<WorkZone>();
            if(workzone != null && !workzone.isOnCooldown()){
                workzone.StartCooldown();
                SpendEnergy(1f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("PlayZone")){
            isRecharging = false;
        }
    }

    IEnumerator RechargeEnergy(){
        while(isRecharging && currentEnergy < maxEnergy){
            currentEnergy += energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy,0, maxEnergy);
            yield return null;
        }
    }

    void SpendEnergy(float amount){
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    //void UpdateEnergyBar(){
    //    if(EnergyBar != null){
    //        EnergyBar.fillAmount = currentEnergy / maxEnergy;
    //    }
   // }
}