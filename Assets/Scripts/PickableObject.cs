    using UnityEngine;

    public enum ItemType
    {
        Note,
        Key
    }
    public class PickableObject : MonoBehaviour
    {
        public Sprite itemSprite;
        [TextArea]
        public string itemDescription;
        public string itemName;
        public ItemType itemType;
    public GameObject itemPrefab;

    void Reset()
        {
            // auto set collider is trigger
            GetComponent<Collider>().isTrigger = false;
            gameObject.tag = "Pickable";
        }
    }
