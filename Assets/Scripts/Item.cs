using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary> Fairly "generic" item class 
/// (in the sense that I have no clue exactly what your games will need,
/// but this is designed to be usable by pretty much any game. )</summary>
public class Item {

	/// <summary> Databases of items </summary>
	public static readonly Dictionary<string, Item> DB = new Dictionary<string, Item>();
	/// <summary> Load an asset containing JSON data into the item database. </summary>
	public static void LoadIntoDB(TextAsset asset) {
		string json = asset.text;
		JsonObject obj = Json.Parse<JsonObject>(json);
		foreach (var pair in obj) {
			Item item = Json.GetValue<Item>(pair.Value);
			if (item != null) {
				item.id = pair.Key;
				DB[pair.Key] = item;
			}
		}
	}

	/// <summary> ID of the item in the database </summary>
	public string id;
	/// <summary> Display name of the item</summary>
	public string name;
	/// <summary> Subtype of item, eg "Weapon", "Ore", "Healing", "Armor", etc... </summary>
	public string kind;
	/// <summary> Item description. </summary>
	public string desc;
	/// <summary> Base cost of this item in default currency. </summary>
	public int value;
	/// <summary> Rarity of this item. It's up to you how to scale this. </summary>
	public int rarity;
	/// <summary> Is this item stackable? </summary>
	public bool stackable { get { return maxStack != 0; } }
	/// <summary> What is the maximum stack of this item? 
	/// Zero implies unstackable. 
	/// Negative values imply infinite stacking.</summary>
	public int maxStack;
	/// <summary> What stats are associated with this item? </summary>
	public Table stats;
	
	/// <summary> Default constructor </summary>
	public Item() { stats = new Table(); }
	
}
