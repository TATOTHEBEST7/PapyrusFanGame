using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;
        [SerializeField] private string input;

        private void Start()
{
    textHolder = GetComponent<Text>();
    StartCoroutine(WriteText(input, textHolder));
    Debug.Log("Coroutine started");
}


    }
}

