using System.Collections.Generic;

// Decorator to allow the JsonUtility to serialize/deserialize this class.
[System.Serializable]
/// <summary> Class for handling data. Extends <see cref="Dictionary{TKey, TValue}"/> </summary>
public class Table : Dictionary<string, float> {
	/// <summary> Empty constructor </summary>
	public Table() : base() { }
	/// <summary> Copy Constructor </summary>
	/// <param name="source"> Source to copy data from  </param>
	public Table(IDictionary<string, float> source) : base(source) { }
	/// <summary> Accessor that provides a zero for any missing keys. </summary>
	/// <param name="key"> Key in table to read </param>
	/// <returns> Value stored in table, or zero if no value is stored. </returns>
	public new float this[string key] {
		get { return ContainsKey(key) ? ((Dictionary<string, float>)this)[key] : 0; }
		set { ((Dictionary<string, float>)this)[key] = value; if (value == 0) { Remove(key); } }
	}
	/// <summary> Operator to add two tables together. </summary>
	/// <param name="a"> First table </param>
	/// <param name="b"> Second table </param>
	/// <returns> A table that holds the sum of all keys in both tables. </returns>
	public static Table operator +(Table a, Table b) {
		Table c = new Table(a);
		foreach (var pair in b) { c[pair.Key] += pair.Value; }
		return c;
	}
	/// <summary> Operator to add two tables together. </summary>
	/// <param name="a"> First table </param>
	/// <param name="b"> Second table </param>
	/// <returns> A table that holds the difference of all keys in both tables. </returns>
	public static Table operator -(Table a, Table b) {
		Table c = new Table(a);
		foreach (var pair in b) { c[pair.Key] -= pair.Value; }
		return c;
	}
	/// <summary> Operator to multiply two tables together. </summary>
	/// <param name="a"> First table </param>
	/// <param name="b"> Second table </param>
	/// <returns> A table that holds the product of all keys in both tables. </returns>
	public static Table operator *(Table a, Table b) {
		Table c = new Table(a);
		foreach (var pair in b) { c[pair.Key] *= pair.Value; }
		return c;
	}
}