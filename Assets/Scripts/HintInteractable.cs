using UnityEngine;
using System.Collections.Generic;

public class HintInteractable : MonoBehaviour
{
    [TextArea]
    public string hintMessage;
    public List<Sprite> hintIcons; // Assign in Inspector
    public enum HintMode { Toggle, Hold }
    public HintMode mode = HintMode.Toggle;

    private bool isHintVisible = false;
    private HintUIManager currentUI;

    // Call this when player enters range
    public void OnEnterRange(HintUIManager ui)
    {
        if (mode == HintMode.Hold && !isHintVisible)
        {
            ui.ShowHint(hintMessage, hintIcons);
            isHintVisible = true;
            currentUI = ui;
        }
    }

    // Call this when player presses interact
    public void Interact(HintUIManager ui)
    {
        if (!isHintVisible)
        {
            ui.ShowHint(hintMessage, hintIcons);
            isHintVisible = true;
            currentUI = ui;
        }
        else
        {
            ui.HideHint();
            isHintVisible = false;
            currentUI = null;
        }
    }

    // Call this when player leaves range
    public void EndInteract()
    {
        if (isHintVisible && currentUI != null)
        {
            currentUI.HideHint();
            isHintVisible = false;
            currentUI = null;
        }
    }
} 