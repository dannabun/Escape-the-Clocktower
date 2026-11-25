using UnityEngine;

// for cabinets, clock cases, drawers etc. anything that just opens when you use it
// target is optional (for models with no separate door mesh), revealOnOpen gets turned on when opened
public class OpenProp : Interactable
{
    public DoorOpen target;
    public GameObject[] revealOnOpen;
    public AudioSource openSound;
    public string openMessage = "";

    bool opened;

    void Start()
    {
        foreach (var g in revealOnOpen)
            if (g != null) g.SetActive(false);
    }

    public override string GetPrompt(PlayerInteract player)
    {
        if (opened) return "";
        return prompt;
    }

    public override void Interact(PlayerInteract player)
    {
        if (opened) return;
        opened = true;

        if (target != null) target.UnlockAndOpen();
        foreach (var g in revealOnOpen)
            if (g != null) g.SetActive(true);
        if (openSound != null) openSound.Play();
        else SFX.Play("creak", transform.position);
        if (openMessage != "") HUD.Instance.ShowMessage(openMessage);
    }
}
