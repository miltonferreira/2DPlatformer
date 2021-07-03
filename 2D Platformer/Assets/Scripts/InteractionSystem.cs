using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    
    // Detection Point
    public Transform detectionPoint;
    // Detection Radius
    private const float detectionRadius = 0.2f;
    // Detection Layer
    public LayerMask detectionLayer;
    // Cached Trigger Object
    public GameObject detectedObject;

    [Header("Examine Fields")]
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;

    public bool isExamining;


    // lista de itens pegos
    // [Header("Lista")]
    // public List<GameObject> pickedItems = new List<GameObject>();
    
    void Update()
    {
        if(DetectObject()){
            if(InteractInput()){
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);    
    }
    bool InteractInput(){

        return Input.GetKeyDown(KeyCode.I);

    }

    bool DetectObject(){
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if(obj==null){
            detectedObject = null;
            return false;
        } else {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    // Mostra info do item na tela
    public void ExamineItem(Item item){

        if(isExamining){
            examineWindow.SetActive(false);     // esconde mostra tela de info
            isExamining = false;                // deixa player se mover
        } else {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineText.text = item.descriptionText;
            examineWindow.SetActive(true);  // mostra tela de info
            isExamining = true;             // não deixa player se mover
        }
    }

}
