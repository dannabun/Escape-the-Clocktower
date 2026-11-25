using UnityEngine;

// base class for anything the player can press E on
public abstract class Interactable : MonoBehaviour
{
    public string prompt = "Interact";

    // return "" to hide the prompt (ex. door is already open)
    public virtual string GetPrompt(PlayerInteract player)
    {
        return prompt;
    }

    public abstract void Interact(PlayerInteract player);
}
