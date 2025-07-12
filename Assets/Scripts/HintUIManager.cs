using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;       
public class HintUIManager : MonoBehaviour
{
    public GameObject hintPanel; // Assign the panel GameObject in Inspector
    public TMP_Text hintText;        // Assign the Text component in Inspector
    public Transform iconPanel;  // Assign in Inspector (child of hintPanel)
    public GameObject iconImagePrefab; // Assign in Inspector (UI prefab with Image)

    public void ShowHint(string message, List<Sprite> icons = null)
    {
        if (hintText != null)
            hintText.text = message;
        if (hintPanel != null)
            hintPanel.SetActive(true);

        // Clear old icons
        if (iconPanel != null)
        {
            foreach (Transform child in iconPanel)
                Destroy(child.gameObject);

            if (icons != null)
            {
                foreach (var icon in icons)
                {
                    GameObject imgObj = Instantiate(iconImagePrefab, iconPanel);
                    imgObj.GetComponent<Image>().sprite = icon;
                }
            }
        }
    }

    public void HideHint()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
        if (iconPanel != null)
        {
            foreach (Transform child in iconPanel)
                Destroy(child.gameObject);
        }
    }
} 