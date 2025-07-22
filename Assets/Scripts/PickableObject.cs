using UnityEngine;


public class PickableObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite itemSprite;
    [TextArea]
    public string itemDescription;

    void Reset()
    {
        // auto set collider is trigger
        GetComponent<Collider>().isTrigger = false;
        gameObject.tag = "Pickable";
    }
}
