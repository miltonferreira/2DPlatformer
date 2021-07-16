using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectCherry : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            FindObjectOfType<LevelManager>().cole_cherry++;
            gameObject.SetActive(false);
        }
    }
}
