using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoss : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player"){
            AudioManager.instance.PlaySFX("hurt_boss");
            FindObjectOfType<Boss1>().Hurt();      // se player tocar no colisor
            gameObject.SetActive(false);
        }
    }
}
