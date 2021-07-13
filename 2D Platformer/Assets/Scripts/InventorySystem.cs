using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    // Inventory Item Class
    public class InventoryItem{
        public GameObject obj;
        public int stack=1;

        public InventoryItem(GameObject o, int s=1){
            obj=o;
            stack=s;
        }
    }
       
    // lista de items pegos
    public List<InventoryItem> items = new List<InventoryItem>();
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
        // se o item é empilhavel
        if(item.GetComponent<Item>().stackable){
            // checa se tem o item no inventory
            InventoryItem ei = items.Find(x=>x.obj.name==item.name);
            if(ei!=null){
                ei.stack++;
            } else {
                // se o item não é empilhavel
                InventoryItem i = new InventoryItem(item);
                items.Add(i);
            }
        }else{
            // se o item não é empilhavel
            InventoryItem i = new InventoryItem(item);
            items.Add(i);
        }

        Update_UI();        //atualiza UI quando pegar item
    }

    // indica se pode pegar itens
    public bool CanPickUp(){
        // se a quantidade de itens for maior que as imagens
        if(items.Count>=items_images.Length){
            return false;
        }else{
            return true;
        }
    }

    // Refresh elementos do inventory window
    void Update_UI(){
        HideAll();  // oculta todos os items
        for(int i=0;i<items.Count;i++){
            items_images[i].sprite = items[i].obj.GetComponent<SpriteRenderer>().sprite;
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

    // descrição do item
    public void ShowDescription(int id){
        // add sprite na imagem da descrição
        description_Image.sprite = items_images[id].sprite;
        // set the title - se for 1 item
        if(items[id].stack==1){    
            description_Title.text = items[id].obj.name;
        } else {
            // se + que 1
            description_Title.text = items[id].obj.name+" x"+items[id].stack;
        }
        // show the description
        description_Text.text = items[id].obj.GetComponent<Item>().descriptionText;
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
        if(items[id].obj.GetComponent<Item>().type == Item.ItemType.Consumables){
            Debug.Log($"CONSUMED {items[id].obj.name}");
            // Invoke the consume custom event
            items[id].obj.GetComponent<Item>().consumeEvent.Invoke();
            // reduce the stack number
            items[id].stack--;
            // if the stack is zero
            if(items[id].stack==0){
                // destroy the item
                Destroy(items[id].obj, 0.1f);
                // Clear the item from the list
                items.RemoveAt(id);
            }
            // Update UI
            Update_UI();
            
        }
    }
}
