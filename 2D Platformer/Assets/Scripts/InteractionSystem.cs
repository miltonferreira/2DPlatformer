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

    // GrabDrop ------------------------------------
    [Header("GrabDrop")]
    public bool isGrabbing;
    public GameObject grabbedObject;
    public float grabbedObjectYValue;
    public Transform grabPoint;


    // lista de itens pegos
    // [Header("Lista")]
    // public List<GameObject> pickedItems = new List<GameObject>();
    
    void Update()
    {
        if(DetectObject()){
            if(InteractInput()){
                
                // se tiver pegado algo, não deixa interagir com outro item
                if(isGrabbing){
                    GrabDrop(); // drop item para interagir com outro item
                    return;
                }

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

    public void GrabDrop(){
        if(isGrabbing){
            isGrabbing = false;
            grabbedObject.transform.parent = null;
            // ao solta obj deixa no ultimo Y que estava quando era parent
            grabbedObject.transform.position = new Vector3(
                grabbedObject.transform.position.x, grabbedObjectYValue, grabbedObject.transform.position.z);
            
            grabbedObject = null;   // não tem item pego
        } else {
            isGrabbing = true;
            grabbedObject = detectedObject; //
            grabbedObject.transform.parent = transform;
            grabbedObjectYValue = grabbedObject.transform.position.y;
            grabbedObject.transform.localPosition = grabPoint.localPosition;
        }
    }

}
