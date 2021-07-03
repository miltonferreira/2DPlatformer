using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{   
    // lista de items pegos
    public List<GameObject> items = new List<GameObject>();
    [Header("Sessao UI Items")]
    // Inventory System Window
    public GameObject ui_window;
    public Image[]  items_images;
    [Header("UI Item Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public Text description_Title;
    public Text description_Text;

    // checa se inventory está aberto ou nao
    public bool isOpen;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.U)){
            ToggleInventory();
        }
    }

    void ToggleInventory(){
        isOpen=!isOpen;
        ui_window.SetActive(isOpen);
        Update_UI();        //atualiza UI quando pegar item
    }

    // add item na lista Inventory
    public void PickUp(GameObject item){
        items.Add(item);
        Update_UI();        //atualiza UI quando pegar item
    }

    // Refresh elementos do inventory window
    void Update_UI(){
        HideAll();  // oculta todos os items
        for(int i=0;i<items.Count;i++){
            items_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            items_images[i].gameObject.SetActive(true);
        }
    }

    // oculta a UI images
    void HideAll(){
        foreach(var i in items_images){
            i.gameObject.SetActive(false);
        }
        HideDescription();
    }

    public void ShowDescription(int id){
        // add sprite na imagem da descrição
        description_Image.sprite = items_images[id].sprite;
        // set the title
        description_Title.text = items[id].name;
        // show the description
        description_Text.text = items[id].GetComponent<Item>().descriptionText;
        // show the elements
        description_Image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }

    // esconde a descrição do item
    public void HideDescription(){
        description_Image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }

    public void Consume(int id){
        if(items[id].GetComponent<Item>().type == Item.ItemType.Consumables){
            Debug.Log($"CONSUMED {items[id].name}");
            // Invoke the consume custom event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            // destroy the item
            Destroy(items[id], 0.1f);
            // Clear the item from the list
            items.RemoveAt(id);
            // Update UI
            Update_UI();
            
        }
    }
}
