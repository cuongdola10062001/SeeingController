using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStanceUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;

    public Button Button => button;

    public void Setup(RoomProfile room)
    {
        nameText.text = room.roomName;
    }
}
