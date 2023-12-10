using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MostraTesto : MonoBehaviour
{
    [SerializeField] Image txtbox;
    public TMP_Text testoDaMostrare;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone");
            testoDaMostrare.text = "BROTHER! ...  WHERE HAS HE GONE TO?! I MUST FIND HIM, NYEHEHEHEHEH!!";
            txtbox.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger zone");
            txtbox.gameObject.SetActive(false);
        }
    }
}
