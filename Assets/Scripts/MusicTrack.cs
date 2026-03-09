using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTrack : MonoBehaviour
{
    [System.Serializable]
    public class NoteSlot
    {
        public string noteID;   // must match the IDs you store in your list<string>
        public Image image;     // drag the UI Image here
    }

    [Header("UI Slots (size = 5)")]
    public NoteSlot[] slots;

    [Header("Colors")]
    public Color uncollectedColor = new Color(1f, 1f, 1f, 0.35f); // pale
    public Color collectedColor = new Color(1f, 1f, 1f, 1f);     // dark/normal

    /// <summary>
    /// Call this whenever the collected notes change.
    /// </summary>
    public void Refresh(ICollection<string> collectedNoteIDs)
    {
        if (slots == null) return;

        foreach (var slot in slots)
        {
            if (slot == null || slot.image == null) continue;

            bool collected = collectedNoteIDs != null && collectedNoteIDs.Contains(slot.noteID);
            slot.image.color = collected ? collectedColor : uncollectedColor;
        }
    }
}