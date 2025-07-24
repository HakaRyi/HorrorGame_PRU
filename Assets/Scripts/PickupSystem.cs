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
    private AudioManager audioManager;

    void Start()
    {
        cam = Camera.main;
        readPaperUI.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, pickableLayer))
            {
                PickableObject pickable = hit.collider.GetComponent<PickableObject>();
                if (pickable != null)
                {
                    switch (pickable.itemType)
                    {
                        case ItemType.Note:
                            ShowItem(pickable);
                            audioManager?.PlaySFX(audioManager.paperPickupClip, audioManager.vfxAudioSource);
                            break;
                        case ItemType.Key:
                            Pickup(pickable);
                            audioManager?.PlaySFX(audioManager.keyPickupClip, audioManager.vfxAudioSource);
                            break;
                        default:
                            Pickup(pickable);
                            break;
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && isReading)
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
        InventoryManager.Instance.AddItem(item);
        item.gameObject.SetActive(false);
    }
    void Pickup(PickableObject item)
    {
        InventoryManager.Instance.AddItem(item);
        item.gameObject.SetActive(false);
    }
    public void CloseUI()
    {
        readPaperUI.SetActive(false);
        isReading = false;
        Time.timeScale = 1;
    }
}
