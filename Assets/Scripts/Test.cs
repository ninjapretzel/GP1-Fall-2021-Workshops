using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	static readonly string[] BASE_STATS = new string[] { "str", "dex", "int", "vit", "agi", "wis" };

	void Awake() {
		Item bob = new Item();
		bob.name = "bob";
		bob.desc = "repost bob so he will take over the internet";
		bob.value = 9001;
		foreach (var stat in BASE_STATS) { bob.stats[stat] = 5; }

		string json = Json.ToJson(bob);
		Debug.Log(json);
	}
	
	void Start() {
		
	}
	
	void Update() {
		
	}
	
}
