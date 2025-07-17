using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSpeechBubble : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform bubbleAnchor;
    public GameObject bubbleIconImagePrefab;
    public SpeechBubbleReplicaManager otherPlayerReplicaManager; // Assign in Inspector
    public float bubbleDuration = 5f;
    

    public void ShowBubble(List<Sprite> icons)
    {
      Debug.Log("ShowBubble called with " + icons.Count + " icons");
        GameObject bubble = Instantiate(bubblePrefab, bubbleAnchor.position, Quaternion.identity, bubbleAnchor);
      Debug.Log("Bubble instantiated: " + bubble);
      Transform iconPanel = bubble.transform.Find("background panel/iconPanel");
      Debug.Log("iconPanel: " + iconPanel);
      Debug.Log("bubbleIconImagePrefab: " + bubbleIconImagePrefab);

      // Remove any placeholder children
      foreach (Transform child in iconPanel)
          Destroy(child.gameObject);

        foreach (Sprite icon in icons)
        {
          GameObject imgObj = Instantiate(bubbleIconImagePrefab, iconPanel);
          Debug.Log("imgObj: " + imgObj);
            imgObj.GetComponent<Image>().sprite = icon;
        }
      Destroy(bubble, bubbleDuration);

      // Show replica on the other player's HUD
      if (otherPlayerReplicaManager != null)
      {
          otherPlayerReplicaManager.ShowReplica(icons);
      }
    }
}