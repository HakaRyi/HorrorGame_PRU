using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public Transform itemListParent;
    public Transform heldItemPoint;

    public GameObject currentHeldItem = null;

    private List<string> collectedItemNames = new List<string>();
    private List<GameObject> collectedItemPrefabs = new List<GameObject>();
    private int currentItemIndex = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;

        inventoryPanel.SetActive(false);
    }

    void Update()
    {

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectItem(i);
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            int nextIndex = currentItemIndex + (Input.mouseScrollDelta.y > 0 ? 1 : -1);
            if (nextIndex >= collectedItemPrefabs.Count) nextIndex = 0;
            if (nextIndex < 0) nextIndex = collectedItemPrefabs.Count - 1;

            SelectItem(nextIndex);
        }
    }

    public void AddItem(PickableObject item)
    {
        if (item.itemPrefab == null)
        {
            Debug.LogError("itemPrefab not assigned for " + item.name);
            return;
        }

        collectedItemPrefabs.Add(item.itemPrefab);
        collectedItemNames.Add(item.itemName);

        GameObject newSlot = Instantiate(itemSlotPrefab, itemListParent);

        var image = newSlot.transform.Find("ItemImage")?.GetComponent<Image>();
        if (image != null)
            image.sprite = item.itemSprite;

        var text = newSlot.transform.Find("ItemText")?.GetComponent<Text>();
        if (text != null)
            text.text = item.itemName;

        if (!inventoryPanel.activeSelf)
            inventoryPanel.SetActive(true);
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < collectedItemPrefabs.Count)
        {
            currentItemIndex = index;

          
            if (currentHeldItem != null)
                Destroy(currentHeldItem);

           
            GameObject prefab = collectedItemPrefabs[index];
            currentHeldItem = Instantiate(prefab, heldItemPoint);
            currentHeldItem.transform.localPosition = Vector3.zero;
            currentHeldItem.transform.localRotation = Quaternion.identity;

   
            Collider col = currentHeldItem.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Debug.Log("You are holding: " + collectedItemNames[index]);
        }
    }
}
