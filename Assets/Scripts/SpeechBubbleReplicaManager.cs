using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeechBubbleReplicaManager : MonoBehaviour
{
    public GameObject replicaPanel; // Assign in Inspector
    public Transform iconPanel;     // Where icons are instantiated
    public GameObject iconImagePrefab; // Prefab with Image component

    private Coroutine hideRoutine;

    public void ShowReplica(List<Sprite> icons, float duration = 5f)
    {
        // Clear old icons
        foreach (Transform child in iconPanel)
            Destroy(child.gameObject);

        // Add new icons
        foreach (var icon in icons)
        {
            GameObject imgObj = Instantiate(iconImagePrefab, iconPanel);
            imgObj.GetComponent<Image>().sprite = icon;
        }

        replicaPanel.SetActive(true);

        // Hide after duration
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        replicaPanel.SetActive(false);
    }
}