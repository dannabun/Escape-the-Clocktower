using UnityEngine;

public class KeyPickup : Interactable
{
    public string keyId = "key";
    public string displayName = "Key";

    public override void Interact(PlayerInteract player)
    {
        player.inventory.AddKey(keyId, displayName);
        SFX.Play("pickup_key", transform.position);
        HUD.Instance.ShowMessage("Picked up the " + displayName.ToLower() + ".");
        Destroy(gameObject);
    }
}
