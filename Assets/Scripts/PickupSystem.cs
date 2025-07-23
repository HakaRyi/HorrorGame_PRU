using UnityEngine;
using UnityEngine.UI;

public class PickupSystem : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask pickableLayer;
    public GameObject readPaperUI;
    public Image itemImage;
    public Text itemDescriptionText;
    
    private Camera cam;
    private bool isReading = false;

    void Start()
    {
        cam = Camera.main;
        readPaperUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance, pickableLayer))
            {
                PickableObject pickable = hit.collider.GetComponent<PickableObject>();
                if (pickable != null)
                {
                    switch (pickable.itemType)
                    {
                        case ItemType.Note:
                            ShowItem(pickable);
                            InventoryManager.Instance.AddItem(pickable);
                            break;

                        case ItemType.Key:
                            InventoryManager.Instance.AddItem(pickable);
                            break;
                    }

                    Destroy(hit.collider.gameObject); 
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isReading)
        {
            CloseUI();
        }
    }

    void ShowItem(PickableObject item)
    {
        readPaperUI.SetActive(true);
        itemImage.sprite = item.itemSprite;
        itemDescriptionText.text = item.itemDescription;
        isReading = true;

    
        Time.timeScale = 0;
    }

    public void CloseUI()
    {
        readPaperUI.SetActive(false);
        isReading = false;
        Time.timeScale = 1;
    }
}
