using UnityEngine;

// a loose gear the player can carry and put on a peg
public class GearPiece : Interactable
{
    public int gearId = 1;
    public string displayName = "Gear";

    [HideInInspector] public GearSlot slot;
    [HideInInspector] public bool locked;

    public float heldSize = 0.12f;     // how big it looks in your hand

    Collider col;
    Vector3 normalScale;

    void Awake()
    {
        col = GetComponent<Collider>();
        normalScale = transform.localScale;
    }

    void Start()
    {
        // settle onto whatever is under it so gears never sit inside a step or float
        if (slot == null) SettleOnGround(transform.position);
    }

    void SettleOnGround(Vector3 from)
    {
        if (col != null) col.enabled = false;
        if (Physics.Raycast(from + Vector3.up * 1.2f, Vector3.down, out RaycastHit hit, 4f, ~0, QueryTriggerInteraction.Ignore))
            transform.position = hit.point + Vector3.up * (0.1f * normalScale.y + 0.01f);
        if (col != null) col.enabled = true;
    }

    // put the held gear down on the floor in front of the player
    public void Drop(PlayerInteract player)
    {
        Transform cam = player.cam.transform;
        Vector3 flat = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        float dist = 0.9f;
        Vector3 chest = player.transform.position + Vector3.up * 1f;
        if (Physics.Raycast(chest, flat, out RaycastHit wall, 1.2f, ~0, QueryTriggerInteraction.Ignore))
            dist = Mathf.Max(0.2f, wall.distance - 0.35f);    // don't drop it into a wall
        Vector3 spot = player.transform.position + flat * dist;

        transform.SetParent(null, true);
        transform.localScale = normalScale;
        transform.rotation = Quaternion.Euler(90f, cam.eulerAngles.y, 0f);
        transform.position = spot;
        SettleOnGround(spot);

        player.inventory.heldGear = null;
        player.inventory.Refresh();
        SFX.Play("gear_place", transform.position, 0.6f);
    }

    public override string GetPrompt(PlayerInteract player)
    {
        if (locked) return "";
        return slot != null ? "Take gear" : "Pick up " + displayName.ToLower();
    }

    public override void Interact(PlayerInteract player)
    {
        PickUp(player);
    }

    public void PickUp(PlayerInteract player)
    {
        if (locked) return;
        if (player.inventory.heldGear != null)
        {
            HUD.Instance.ShowMessage("My hands are full.");
            return;
        }

        if (slot != null)
        {
            slot.current = null;
            slot = null;
        }

        transform.SetParent(player.holdPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * heldSize;
        if (col != null) col.enabled = false;

        player.inventory.heldGear = this;
        player.inventory.Refresh();
        SFX.Play("pickup", transform.position, 0.7f);
    }

    public void PlaceOn(GearSlot s, PlayerInteract player)
    {
        transform.SetParent(s.snapPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = normalScale;
        if (col != null) col.enabled = true;

        slot = s;
        s.current = this;
        SFX.Play("gear_place", transform.position, 0.8f);
        player.inventory.heldGear = null;
        player.inventory.Refresh();
    }
}
