using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TextSystem : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] prompts;
    public string sceneToLoad;

    public float delayBetweenCharacters = 0.05f;
    public float delayAfterText = 1f;
    public float shakeIntensity = 0.5f;
    public float shakeSpeed = 50f;

    public AudioSource audioSource;
    public AudioClip[] letterSounds; // Array of letter sounds
    public AudioClip promptStartSound;

    public GameObject[] selectableGameObjects; // Array of selectable game objects

    public GameObject noResponseGameObject; // GameObject to be shown when no response is given

    private Dictionary<string, GameObject> promptGameObjectMap; // Map prompts to game objects

    private Vector3 originalPosition;
    private int currentPromptIndex;
    private bool isAnimating;

    private void Start()
    {
        originalPosition = textMeshPro.rectTransform.localPosition;
        currentPromptIndex = 0;
        isAnimating = false;

        // Create the prompt to game object mapping
        CreatePromptGameObjectMap();

        GenerateText();
    }

    private void CreatePromptGameObjectMap()
    {
        promptGameObjectMap = new Dictionary<string, GameObject>();

        // Map prompts to game objects based on index
        for (int i = 0; i < prompts.Length; i++)
        {
            if (i < selectableGameObjects.Length)
            {
                string prompt = prompts[i];
                GameObject gameObject = selectableGameObjects[i];

                promptGameObjectMap.Add(prompt, gameObject);
            }
        }
    }

    private void GenerateText()
    {
        if (currentPromptIndex >= prompts.Length)
        {
            // All prompts displayed, load the specified scene
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        string currentPrompt = prompts[currentPromptIndex];
        textMeshPro.text = ReplaceComputerName(currentPrompt);

        if (currentPromptIndex > 0 && audioSource != null && promptStartSound != null)
        {
            audioSource.PlayOneShot(promptStartSound);
        }

        // Hide all selectable game objects
        foreach (GameObject obj in selectableGameObjects)
        {
            obj.SetActive(false);
        }

        // Display the specific game object for the current prompt
        if (promptGameObjectMap.TryGetValue(currentPrompt, out GameObject gameObject))
        {
            gameObject.SetActive(true);
        }

        // Show the no response game object if the prompt is empty
        noResponseGameObject.SetActive(string.IsNullOrEmpty(currentPrompt));

        StartCoroutine(AnimateText(currentPrompt));
    }

    private System.Collections.IEnumerator AnimateText(string prompt)
    {
        isAnimating = true;

        int currentIndex = 0;
        while (currentIndex < prompt.Length)
        {
            textMeshPro.text = ReplaceComputerName(prompt.Substring(0, currentIndex + 1));
            currentIndex++;

            textMeshPro.rectTransform.localPosition = originalPosition + GetShakeOffset();

            if (audioSource != null && letterSounds.Length > 0)
            {
                // Play a random letter sound from the array
                int randomSoundIndex = Random.Range(0, letterSounds.Length);
                audioSource.PlayOneShot(letterSounds[randomSoundIndex]);
            }

            yield return new WaitForSeconds(delayBetweenCharacters);
        }

        textMeshPro.rectTransform.localPosition = originalPosition + GetShakeOffset();

        yield return new WaitForSeconds(delayAfterText);

        isAnimating = false;
    }

    private Vector3 GetShakeOffset()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed) * shakeIntensity;

        return new Vector3(offsetX, offsetY, 0f);
    }

    private string ReplaceComputerName(string prompt)
    {
        string computerName = SystemInfo.deviceName;
        return prompt.Replace("/./", computerName);
    }

    private void Update()
    {
        if (!isAnimating && Input.GetMouseButtonDown(0))
        {
            // Proceed to the next prompt when the mouse is clicked, but only if the current prompt is not empty
            if (!string.IsNullOrEmpty(prompts[currentPromptIndex]))
            {
                currentPromptIndex++;
                GenerateText();
            }
        }
    }
}

