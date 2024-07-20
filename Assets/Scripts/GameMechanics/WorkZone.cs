using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkZone : MonoBehaviour
{

    public float coolDownTime = 5f;
    public bool onCooldown = false;

    public bool isOnCooldown(){
        return onCooldown;
    }

    public void StartCooldown(){
        if(!onCooldown){
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown(){
        onCooldown = true;
        yield return new WaitForSeconds(coolDownTime);
        onCooldown = false;
    }
}
