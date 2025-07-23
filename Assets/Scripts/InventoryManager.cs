using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public GameObject inventoryPanel;        
    public GameObject itemSlotPrefab;         
    public Transform itemListParent;         

    private List<string> collectedItemNames = new List<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;

        inventoryPanel.SetActive(false);
    }

    public void AddItem(PickableObject item)
    {

        GameObject newSlot = Instantiate(itemSlotPrefab, itemListParent);

      
        var image = newSlot.transform.Find("ItemImage")?.GetComponent<Image>();
        if (image != null)
            image.sprite = item.itemSprite;

        
        var text = newSlot.transform.Find("ItemText")?.GetComponent<Text>();
        if (text != null)
            text.text = item.itemName;

        collectedItemNames.Add(item.itemName);

        if (!inventoryPanel.activeSelf)
            inventoryPanel.SetActive(true);
    }
}
