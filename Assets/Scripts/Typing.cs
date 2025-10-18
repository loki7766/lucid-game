using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    public Text uiText;
    public float typingSpeed = 0.05f;

    private string fullText;

    void Start()
    {
        
        fullText = uiText.text;

     
        uiText.text = "";

 
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            uiText.text += letter; 
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
