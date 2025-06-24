using UnityEngine;

public class DoorController : MonoBehaviour 
{
    public Animator doorAnimator;
    public bool isPlayerNear = false;

    private void Update()
    {
        if(isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed - Trying to open the door");
            doorAnimator.SetTrigger("Open");
        }
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

     void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
