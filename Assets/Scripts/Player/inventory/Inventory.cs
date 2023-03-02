using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
	public ItemDatabase itemDatabase;
	
	public void Start()
	{
		GiveItem(0);
	}
	
	public void GiveItem(int id)
	{
		Item itemToAdd = itemDatabase.GetItem(id);
		characterItems.Add(itemToAdd);
		Debug.Log("Gived item '" + itemToAdd.title + "' to player1");
	}
}
