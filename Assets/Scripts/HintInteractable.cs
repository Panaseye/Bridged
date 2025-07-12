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

    public void Interact(HintUIManager ui)
    {
        if (mode == HintMode.Toggle)
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
        else if (mode == HintMode.Hold)
        {
            ui.ShowHint(hintMessage, hintIcons);
            isHintVisible = true;
            currentUI = ui;
        }
    }

    // Always called when player leaves, regardless of mode
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