using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public float health;

    // 100 health = 1 fill
    // 45 health = 0.45 fill

    public void LoseHealth(int value){

        // não deixa executa codigo abaixo
        if(health<=0)
            return;
        // reduz health
        health -= value;
        // atualiza o fillBar
        fillBar.fillAmount = health / 100;
        // se fillBar zerar GameOver
        if(health<=0){
            FindObjectOfType<Player>().Die();
        }
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Return)){
        //     LoseHealth(25);
        // }
    }

}
