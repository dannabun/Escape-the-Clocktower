using UnityEngine;
using UnityEngine.InputSystem;

// raycast from the camera, show a prompt for whatever we're looking at, press E to use it
public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public Transform holdPoint;
    public Inventory inventory;
    public float range = 2.6f;

    Interactable current;

    // what the player is looking at right now (used by puzzles that have a second key)
    public static Interactable Current;

    void Update()
    {
        if (HUD.Instance == null) return;

        if (Time.timeScale == 0f || !GameManager.CanPlay || HUD.Instance.BlockingInput)
        {
            HUD.Instance.SetPrompt("");
            Current = null;
            return;
        }

        current = null;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range, ~0, QueryTriggerInteraction.Ignore))
            current = hit.collider.GetComponentInParent<Interactable>();

        // small things (gears, keys) are fiddly to hit exactly, so give the aim a little slack
        if (current == null && Physics.SphereCast(ray, 0.18f, out RaycastHit near, range, ~0, QueryTriggerInteraction.Ignore))
            current = near.collider.GetComponentInParent<Interactable>();

        Current = current;

        // G drops whatever gear you're carrying
        if (inventory.heldGear != null && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
            inventory.heldGear.Drop(this);
        string prompt = current != null ? current.GetPrompt(this) : "";
        HUD.Instance.SetPrompt(prompt);

        if (prompt != "" && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            current.Interact(this);
    }
}
