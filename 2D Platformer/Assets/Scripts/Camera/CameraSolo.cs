using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSolo : MonoBehaviour
{
    public Transform target;        // obj que cam vai focar
    public Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            FindObjectOfType<DynamicCameraFollow>().target = target;    // troca o foco da cam
            FindObjectOfType<Player>().isNotMovePlayer = true;          // nao deixa player se mover
            StartCoroutine(returnPlayer(2f));
        }
    }

    IEnumerator returnPlayer(float time){
        yield return new WaitForSeconds(time);
        FindObjectOfType<DynamicCameraFollow>().target = player;    // troca o foco da cam
        FindObjectOfType<Player>().isNotMovePlayer = false;         // deixa player se mover
        gameObject.SetActive(false);
        
        
    }
}
