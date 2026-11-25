using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<string> keys = new List<string>();
    List<string> keyNames = new List<string>();

    // only one gear can be carried at a time
    [HideInInspector] public GearPiece heldGear;

    public void AddKey(string id, string displayName)
    {
        if (keys.Contains(id)) return;
        keys.Add(id);
        keyNames.Add(displayName);
        Refresh();
    }

    public bool HasKey(string id)
    {
        return keys.Contains(id);
    }

    public void Refresh()
    {
        if (HUD.Instance == null) return;

        string s = "";
        if (keyNames.Count > 0) s += "Keys: " + string.Join(", ", keyNames);
        if (heldGear != null)
        {
            if (s != "") s += "\n";
            s += "Holding: " + heldGear.displayName + "  (G to drop)";
        }
        HUD.Instance.SetInventory(s);
    }
}
