using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    public GameObject UINextLevel;

    private void OnTriggerEnter2D(Collider2D col) {

        int lives = FindObjectOfType<LifeCount>().livesRemaining;

        if(col.gameObject.tag == "Player" && lives>0){
            AudioManager.instance.PlaySFX("end");
            FindObjectOfType<Player>().isNotMovePlayer = true;
            UINextLevel.SetActive(true);
        }
    }

    public void BtnAction(string var){
        if(var == "exit"){
            Application.Quit();
        }
        if(var == "restart"){
             SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }
    }
}
