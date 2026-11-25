using UnityEngine;

// goes on the door mesh. if requiredKey is empty the door is opened by something else (like a puzzle)
public class LockedDoor : Interactable
{
    public DoorOpen door;
    public string requiredKey = "";
    public string lockedMessage = "It's locked.";

    public override string GetPrompt(PlayerInteract player)
    {
        if (door == null || door.IsOpen) return "";
        return prompt;
    }

    public override void Interact(PlayerInteract player)
    {
        if (door.IsOpen) return;

        if (requiredKey != "")
        {
            if (player.inventory.HasKey(requiredKey))
            {
                door.UnlockAndOpen();
                HUD.Instance.ShowMessage("The key turns. The door swings open.");
            }
            else
            {
                SFX.Play("locked", transform.position, 0.6f);
                HUD.Instance.ShowMessage(lockedMessage);
            }
        }
        else if (door.isLocked)
        {
            SFX.Play("locked", transform.position, 0.6f);
            HUD.Instance.ShowMessage(lockedMessage);
        }
        else
        {
            door.TryOpen();
        }
    }
}
