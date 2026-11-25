using UnityEngine;

public class GearSlot : Interactable
{
    public int slotNumber = 1;   // which gear id belongs here
    public Transform snapPoint;
    public GearPuzzle puzzle;          // room style puzzle (checks when a gear is placed)
    public GearTrainPuzzle train;      // level 2 lever puzzle (checks when the lever is pulled)

    [HideInInspector] public GearPiece current;

    bool Solved()
    {
        return (puzzle != null && puzzle.solved) || (train != null && train.solved);
    }

    public override string GetPrompt(PlayerInteract player)
    {
        if (Solved()) return "";
        if (current != null) return "Take gear";
        if (player.inventory.heldGear != null) return "Place " + player.inventory.heldGear.displayName.ToLower();
        return "Empty peg";
    }

    public override void Interact(PlayerInteract player)
    {
        if (Solved()) return;

        if (current != null)
        {
            current.PickUp(player);
            return;
        }

        if (player.inventory.heldGear == null)
        {
            HUD.Instance.ShowMessage("Looks like a gear goes here.");
            return;
        }

        player.inventory.heldGear.PlaceOn(this, player);
        if (puzzle != null) puzzle.Check();
    }

    public bool IsCorrect()
    {
        return current != null && current.gearId == slotNumber;
    }
}
