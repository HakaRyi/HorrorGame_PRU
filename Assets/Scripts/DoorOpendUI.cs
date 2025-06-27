using UnityEngine;

public class DoorOpendUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseOver()
    {
        UIController.actionText = "Opend";
        UIController.commandText = "E";
        UIController.isActive = true;
    }

    private void OnMouseExit()
    {
        UIController.actionText = "";
        UIController.commandText = "";
        UIController.isActive = false;
    }
}
