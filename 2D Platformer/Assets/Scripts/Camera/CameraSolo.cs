using System.Collections;
using UnityEngine;

public class CameraSolo : MonoBehaviour
{
    public Transform target;        // obj que cam vai focar
    public Transform player;

    [Header("Colide com Player?")]
    public bool isColl;         // indica que tem colisão com player
    
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player" && isColl){
           changeFocus();   // muda foco da camera
        }
    }

    public void changeFocus(){
        FindObjectOfType<DynamicCameraFollow>().target = target;    // troca o foco da cam
        FindObjectOfType<Player>().isNotMovePlayer = true;          // nao deixa player se mover
        StartCoroutine(returnPlayer(2f));
    }

    IEnumerator returnPlayer(float time){
        yield return new WaitForSeconds(time);
        FindObjectOfType<DynamicCameraFollow>().target = player;    // troca o foco da cam
        FindObjectOfType<Player>().isNotMovePlayer = false;         // deixa player se mover
        
        if(isColl)
        gameObject.SetActive(false);
        
        
    }
}
