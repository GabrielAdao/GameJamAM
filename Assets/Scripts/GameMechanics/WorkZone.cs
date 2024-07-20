using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorkZone : MonoBehaviour
{

    public float coolDownTime = 10f;
    public Color activeColor = Color.green;
    public Color cooldownColor = Color.red;
    public TextMeshProUGUI cooldownText;
    public bool onCooldown = false;
    public float cooldownTimer = 0f;


    private void Start() {
        if(cooldownText != null){
            cooldownText.text = "";
        }
        UpdateZoneColor();
    }

    private void Update() {
        if(onCooldown){
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();

            if(cooldownTimer <= 0f){
                onCooldown = false;
                cooldownText.text = "";
                UpdateZoneColor();
            }
        }
    }

    public bool isOnCooldown(){
        return onCooldown;
    }

    public void StartCooldown(){
        if(!onCooldown){
            onCooldown = true;
            cooldownTimer = coolDownTime;
            UpdateZoneColor();
        }
    }

    void UpdateZoneColor(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.color = onCooldown ? cooldownColor : activeColor;
        }
    }
}
