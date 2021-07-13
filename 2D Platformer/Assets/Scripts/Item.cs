using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    public enum InteractionType { NONE, PickUp, Examine, GrabDrop}

    [Header("Tipo do Item")]
    public InteractionType interactType;
    public enum ItemType { Static, Consumables}
    public ItemType type;

    public bool stackable = false;

    [Header("Examine")]
    public string descriptionText;

    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }

    public void Interact(){
        switch(interactType){
            case InteractionType.PickUp:
                // Se não pode pegar item, não executa o resto do codigo
                if(!FindObjectOfType<InventorySystem>().CanPickUp())
                    return;
                // add item na lista de itens coletados
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                // infos sobre o item
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                Debug.Log("Examine");
                break;
            case InteractionType.GrabDrop:
                FindObjectOfType<InteractionSystem>().GrabDrop();
                break;
            default:
                Debug.Log("Null Item");
                break;
        }

        // chama event
        customEvent.Invoke();
    }

}
