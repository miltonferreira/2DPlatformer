using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives;
    public int livesRemaining;

    public void LoseLife(){
        // se zerar vidas não deixa executar codigo abaixo
        if(livesRemaining==0)
            return;
        // decrementa o valor de livesRemaining
        livesRemaining--;
        // esconde uma imagem da vida
        lives[livesRemaining].enabled = false;
        // GameOver
        if(livesRemaining==0){
            FindObjectOfType<Player>().Die();
        }
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Return)){
        //     LoseLife();
        // }
    }
}
