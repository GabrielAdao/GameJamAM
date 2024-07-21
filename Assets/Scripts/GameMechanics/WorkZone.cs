using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorkZone : MonoBehaviour
{

    public float coolDownTime = 10f;
    public float holdTimeRequired = 5f;
    public Color activeColor = Color.green;
    public Color cooldownColor = Color.red;
    public Color holdColor = Color.yellow;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI holdText;

    private bool onCooldown = false;
    private float cooldownTimer = 0f;
    private float holdTimer = 0;
    private bool isPlayerInZone = false;
    private bool isHolding = false; 
    


    private void Start() {
        if(cooldownText != null){
            cooldownText.text = "";
        }
        if(holdText != null){
            holdText.text = "";
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

        }else if(isPlayerInZone && Input.GetKey(KeyCode.Space)){
            holdTimer += Time.deltaTime;
            holdText.text = Mathf.Ceil(holdTimeRequired - holdTimer).ToString();
            isHolding = true;
            UpdateZoneColor();

            if(holdTimer >= holdTimeRequired){
                HandleWorkZone(null);
            }

        }else if(isPlayerInZone && !Input.GetKey(KeyCode.Space)){
            if(isHolding){
                holdTimer = 0f;
                holdText.text = "";
                isHolding = false;
                UpdateZoneColor();
            }
        } 
    }

    public bool IsOnCooldown(){
        return onCooldown;
    }

    public void HandleWorkZone(PlayerEnergy playerEnergy){
        playerEnergy = FindObjectOfType<PlayerEnergy>();
        if(!onCooldown && holdTimer >= holdTimeRequired){
            playerEnergy.SpendEnergy(1f);
            StartCooldown();
        }
    }

    public void StartCooldown(){
        if(!onCooldown){
            onCooldown = true;
            cooldownTimer = coolDownTime;
            holdTimer = 0f;
            holdText.text = "";
            isHolding = false;
            UpdateZoneColor();
        }
    }

    void UpdateZoneColor(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            if(onCooldown){
                sr.color = cooldownColor;
            }else if(isPlayerInZone && isHolding){
                sr.color = holdColor;
            }else{
                sr.color = activeColor;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            isPlayerInZone = true;
            holdTimer = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            isPlayerInZone = false;
            holdTimer = 0f;
            holdText.text = "";
            isHolding = false;
            UpdateZoneColor();
        }
    }
}
