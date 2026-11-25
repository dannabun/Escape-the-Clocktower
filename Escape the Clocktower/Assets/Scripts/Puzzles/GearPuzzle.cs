using UnityEngine;

// room 2. all three gears on the right pegs = door opens
public class GearPuzzle : MonoBehaviour
{
    public GearSlot[] slots;
    public DoorOpen door;
    public SpinGear[] spinners;
    public float solvedSpinMultiplier = 4f;

    [HideInInspector] public bool solved;

    public void Check()
    {
        if (solved) return;

        int filled = 0;
        int correct = 0;
        foreach (var s in slots)
        {
            if (s.current != null) filled++;
            if (s.IsCorrect()) correct++;
        }

        if (correct == slots.Length)
        {
            solved = true;
            foreach (var s in slots) s.current.locked = true;
            foreach (var g in spinners) g.speed *= solvedSpinMultiplier;
            if (door != null) door.UnlockAndOpen();
            HUD.Instance.ShowMessage("The mechanism grinds to life. A door opens somewhere ahead.", 4f);
        }
        else if (filled == slots.Length)
        {
            HUD.Instance.ShowMessage("The gears jam with a horrible screech. Wrong order?");
        }
    }
}
