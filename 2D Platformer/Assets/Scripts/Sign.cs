using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject dialogOBJ;
    public Text text;
    
    public string[] dialog;
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D collison) {
        if(collison.tag == "Player"){
            dialogOBJ.SetActive(true);
            if(FindObjectOfType<LevelManager>().isChoice){
                text.text = dialog[1].ToString();
            }else{
                text.text = dialog[0].ToString();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collison) {
        if(collison.tag == "Player"){
            dialogOBJ.SetActive(false);
            text.text = null;
        }
    }
}
