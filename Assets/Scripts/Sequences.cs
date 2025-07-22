using System.Collections;
using System.Runtime.ExceptionServices;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Sequences : MonoBehaviour
{
    public GameObject PlayerScripts;
    public GameObject FadeScreenIn;
    public GameObject TextBox;
    void Start()
    {
        PlayerScripts.GetComponent<FirstPersonController>().enabled = false;
        StartCoroutine(ScenePlayer());
    }
    IEnumerator ScenePlayer()
    {
        yield return new WaitForSeconds(1.5f);
        FadeScreenIn.SetActive(false);

        string fullText = "Your mission: Find key and get outta here!";
        TextBox.GetComponent<Text>().text = "";  

        foreach (char letter in fullText.ToCharArray())
        {
            TextBox.GetComponent<Text>().text += letter;
            yield return new WaitForSeconds(0.05f); 
        }

        yield return new WaitForSeconds(2f);
        TextBox.GetComponent<Text>().text = "";
        PlayerScripts.GetComponent<FirstPersonController>().enabled = true;
    }

}
