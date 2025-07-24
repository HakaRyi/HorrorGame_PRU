using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text textBox;
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    void Start()
    {
        UIController.LockCursor(); 
    }
    public void ShowDialogue(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        textBox.text = "";
        foreach (char c in message.ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2f);
        textBox.text = "";
    }
}
