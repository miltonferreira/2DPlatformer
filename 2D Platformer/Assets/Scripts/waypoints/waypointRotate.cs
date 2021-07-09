using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class waypointRotate : MonoBehaviour
{
    
    public float up = -90;
    private bool isToggle;       // troca rotação
    public bool isUp;
    public bool isRotationZero;


    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if(!isRotationZero && isUp){
            if(collider.tag == "Opossum"){
                collider.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, up);
            }
        } else if(isRotationZero && !isUp){
            if(collider.tag == "Opossum"){
                collider.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Opossum"){
            isRotationZero = !isRotationZero;
            isUp = !isUp;
        }
    }
}
