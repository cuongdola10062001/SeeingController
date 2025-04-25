using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{
    [SerializeField] private GameObject itemStanceParent;
    [SerializeField] private GameObject itemStancePrefab;

    [SerializeField] private Player player;

    private List<ItemStanceUI> itemStances = new List<ItemStanceUI>();

    private void OnEnable()
    {
        player.OnChangeRoomIndex += this.ChangeInteractiveItem;
    }
    private void OnDisable()
    {
        player.OnChangeRoomIndex -= this.ChangeInteractiveItem;
    }

    public void Setup(List<RoomProfile> roomList)
    {
        this.InstantiateStances(roomList);
    }

    private void InstantiateStances(List<RoomProfile> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject newItemStance = Instantiate(itemStancePrefab, itemStanceParent.transform);
            ItemStanceUI itemStanceUI = newItemStance.GetComponent<ItemStanceUI>();
            itemStanceUI.Setup(roomList[i]);
            itemStances.Add(itemStanceUI);
        }

        this.ChangeInteractiveItem(player.CurrentRoomIndex);
    }

    public void ChangeInteractiveItem(int index)
    {
        this.DisInteractiveItems();
        itemStances[index].Button.interactable = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void DisInteractiveItems()
    {
        for (int i = 0; i < itemStances.Count; i++)
        {
            itemStances[i].Button.interactable = false;
        }
    }
}
