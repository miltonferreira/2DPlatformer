using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject dialogOBJ;
    public Text text;
    
    public string[] dialog;
    
    [Header("ultimo diag scene")]
    public bool isFinalLevel;    // indica se é o aviso do final da fase
    
    private void OnTriggerEnter2D(Collider2D collison) {
        if(collison.tag == "Player"){
            dialogOBJ.SetActive(true);
            if(FindObjectOfType<LevelManager>().isChoice){
                text.text = dialog[1].ToString();
            }else{
                text.text = dialog[0].ToString();
            }
        }

        if(isFinalLevel){
            FindObjectOfType<Player>().isNotMovePlayer = true;   // nao deixa player se mover
        }
    }

    private void OnTriggerExit2D(Collider2D collison) {
        if(collison.tag == "Player"){
            dialogOBJ.SetActive(false);
            text.text = null;
        }
    }
}
