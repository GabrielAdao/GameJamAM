using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeSystem : MonoBehaviour
{
    public int playerMaxLife;
    public int playerCurrentLife;

    private void Start() {
        playerCurrentLife = playerMaxLife;
    }
    public void PlayerLoseLife(){
        playerCurrentLife -= 1;
        if(playerCurrentLife <= 0){
            PlayerDeath();
        }
    }



    void PlayerDeath(){
        Debug.Log("Change Scene");
    }
}
