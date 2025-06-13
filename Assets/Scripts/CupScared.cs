using UnityEngine;

public class CupScared : MonoBehaviour
{
    public GameObject Cup;

    void OnTriggerEnter(Collider other)
    {
        Cup.SetActive(true);

        Rigidbody rb = Cup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
