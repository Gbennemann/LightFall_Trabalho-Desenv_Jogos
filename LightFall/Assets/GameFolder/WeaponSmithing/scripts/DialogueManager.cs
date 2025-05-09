using Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private string[] dialogueLines;
    private float textSpeed = 0.3f;
    private int index;
    private bool isPlayerInRange = false;

    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    private AttackCollider attackCollider;

    private Image powerUpActiveObject;


    void Start()
    {
        dialogueText.text = string.Empty;
        attackCollider = FindObjectOfType<AttackCollider>();
        powerUpActiveObject = GameObject.Find("PowerUpActive").GetComponent<Image>();
        StartDialogue();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
                
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (powerUpActiveObject.enabled == true) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    IEnumerator TypeLine()
    {
        foreach (var c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void ActivePopUp()
    {
        dialogueBox.SetActive(true);
        yesButton.onClick.AddListener(UpgradeWeapon);
        noButton.onClick.AddListener(CloseUpgradePopup);
    }

    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = string.Empty;
            index = 0;
            ActivePopUp();
        }
    }

    void UpgradeWeapon()
    {
        attackCollider.UpgradeWeapon(1);
        powerUpActiveObject = GameObject.Find("PowerUpActive").GetComponent<Image>();
        powerUpActiveObject.enabled = true;
        Debug.Log("Weapon Upgraded!");
      
        CloseUpgradePopup();
    }

    void CloseUpgradePopup()
    {
        dialogueBox.SetActive(false);
    }


}
