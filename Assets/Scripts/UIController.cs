using UnityEngine;

public class UIController : MonoBehaviour
{
    public static string actionText;
    public static string commandText;
    public static bool isActive;
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject commandBox;
    [SerializeField] GameObject interactiveCross;
    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            actionBox.SetActive(true);
            commandBox.SetActive(true);
            interactiveCross.SetActive(true);
            actionBox.GetComponent<TMPro.TMP_Text>().text = actionText;
            commandBox.GetComponent<TMPro.TMP_Text>().text = commandText;
        }
        else
        {
            actionBox.SetActive(false);
            commandBox.SetActive(false);
            interactiveCross.SetActive(false);
        }
    }
}
