using System.Collections;
using UnityEngine;

// pull it and it asks the gear train to run. if the gears are wrong it springs back up
public class Lever : Interactable
{
    public Transform handle;
    public Vector3 pulledRotation = new Vector3(60f, 0f, 0f);  // added to the handle's rotation
    public GearTrainPuzzle puzzle;

    Quaternion upRot;
    bool busy;
    bool done;

    void Start()
    {
        if (handle != null) upRot = handle.localRotation;
    }

    public override string GetPrompt(PlayerInteract player)
    {
        if (done || busy) return "";
        return prompt;
    }

    public override void Interact(PlayerInteract player)
    {
        if (!busy && !done) StartCoroutine(Pull());
    }

    IEnumerator Pull()
    {
        busy = true;
        SFX.Play("tick", transform.position);
        yield return Move(upRot, upRot * Quaternion.Euler(pulledRotation), 0.35f);

        bool ok = puzzle != null && puzzle.TryRun();
        if (ok)
        {
            done = true;
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
            yield return Move(handle.localRotation, upRot, 0.5f);
        }
        busy = false;
    }

    IEnumerator Move(Quaternion from, Quaternion to, float time)
    {
        if (handle == null) yield break;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            handle.localRotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0, 1, t / time));
            yield return null;
        }
        handle.localRotation = to;
    }
}
