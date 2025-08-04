using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuickChatUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject trayPanel;
    public Button quickChatButton;
    public Transform messageLinePanel; // Parent with HorizontalLayoutGroup
    public GameObject iconImagePrefab; // Prefab with an Image component
    public Button sendButton;
    public PlayerSpeechBubble playerSpeechBubble;

    [Header("Icon Grid")]
    public Transform iconGridPanel; // Where icon buttons are instantiated
    public GameObject iconButtonPrefab; // Prefab for icon selection buttons
    public List<Sprite> availableIcons; // Assign your icon sprites here
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip sendSound;
    [SerializeField] private AudioClip toggleSound;

    [Header("Bubble Icon Image Prefab")]
    public GameObject bubbleIconImagePrefab; // Assign in Inspector

    private List<Sprite> selectedIcons = new List<Sprite>();

    void Start()
    {
        trayPanel.SetActive(false);
        quickChatButton.onClick.AddListener(ToggleTray);
        sendButton.onClick.AddListener(SendMessage);
        PopulateIconGrid();
    }

    public void ToggleTray()
    {
        trayPanel.SetActive(!trayPanel.activeSelf);
        audioSource.PlayOneShot(toggleSound, 0.5f);  
    }

    public void AddIconToMessage(Sprite icon)
    {
        selectedIcons.Add(icon);
        audioSource.PlayOneShot(clickSound, 0.5f);
        Debug.Log("Added icon: " + icon.name + " | Total: " + selectedIcons.Count);
        GameObject imgObj = Instantiate(iconImagePrefab, messageLinePanel);
        imgObj.GetComponent<Image>().sprite = icon;

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageLinePanel);
    }

    public void ClearMessageLine()
    {
        foreach (Transform child in messageLinePanel)
            Destroy(child.gameObject);
        selectedIcons.Clear();
    }

    public void SendMessage()
    {
    Debug.Log("SendMessage called. selectedIcons.Count = " + selectedIcons.Count);
    if (selectedIcons.Count > 0 && playerSpeechBubble != null)
        {
        playerSpeechBubble.ShowBubble(selectedIcons);
        audioSource.PlayOneShot(sendSound, 0.5f);
            ClearMessageLine();
            trayPanel.SetActive(false);
        }
    else
    {
        Debug.LogWarning("SendMessage: No icons or playerSpeechBubble is null.");
    }
    }

    private void PopulateIconGrid()
    {
        foreach (Sprite icon in availableIcons)
        {
            GameObject btnObj = Instantiate(iconButtonPrefab, iconGridPanel);
            btnObj.GetComponent<Image>().sprite = icon;
            btnObj.GetComponent<Button>().onClick.AddListener(() => AddIconToMessage(icon));
        }
    }
}