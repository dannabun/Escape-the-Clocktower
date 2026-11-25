using UnityEngine;

public class NoteReader : Interactable
{
    public string title = "Note";
    [TextArea(4, 12)] public string body;

    public override void Interact(PlayerInteract player)
    {
        SFX.Play("note_open", transform.position, 0.8f);
        HUD.Instance.ShowNote(title, body);
    }
}
