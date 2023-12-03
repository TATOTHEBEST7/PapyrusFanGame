using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        protected IEnumerator WriteText(string input, Text textHolder)
{
    for (int i = 0; i < input.Length; i++)
    {
        textHolder.text = input.Substring(0, i + 1); // Set the entire text up to the current character
        Debug.Log($"Current Text: {textHolder.text}");
        yield return new WaitForSeconds(0.1f);
    }
}


    }
}

