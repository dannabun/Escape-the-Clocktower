using UnityEngine;

// Level 2: three pegs in a row - drive, middle, great wheel. The great wheel has to turn
// once for every 3 turns of the drive. In a simple gear train only the first and last gear
// matter (ratio = last teeth / first teeth), the middle one just passes the motion along.
public class GearTrainPuzzle : MonoBehaviour
{
    public GearSlot driveSlot;
    public GearSlot middleSlot;
    public GearSlot wheelSlot;
    public float targetRatio = 3f;
    public ClockMechanism mechanism;

    [HideInInspector] public bool solved;

    // called by the lever. returns true if the tower starts
    public bool TryRun()
    {
        if (solved) return true;

        if (driveSlot.current == null || middleSlot.current == null || wheelSlot.current == null)
        {
            SFX.Play("locked", transform.position, 0.7f);
            HUD.Instance.ShowMessage("The lever clunks but nothing catches. All three pegs need a gear.", 3f);
            return false;
        }

        float drive = driveSlot.current.gearId;
        float wheel = wheelSlot.current.gearId;
        float ratio = wheel / drive;

        if (Mathf.Abs(ratio - targetRatio) < 0.01f)
        {
            solved = true;
            driveSlot.current.locked = true;
            middleSlot.current.locked = true;
            wheelSlot.current.locked = true;
            if (mechanism != null) mechanism.StartMechanism();
            HUD.Instance.ShowMessage("The gears bite. The whole tower shudders awake, and far above, the belfry hatch grinds open. Get to the top.", 5f);
            return true;
        }

        SFX.Play("hit", transform.position, 0.6f);
        PlayerController.Shake(0.06f, 0.4f);
        string how = ratio > targetRatio ? "too slow" : "too fast";
        HUD.Instance.ShowMessage("The great wheel lurches " + how + " - once every " + ratio.ToString("0.##") +
                                 " turns of the drive - and jams.", 4f);
        return false;
    }
}
