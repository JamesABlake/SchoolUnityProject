using System;
using System.Runtime.CompilerServices;

[Serializable]
public class UTuple<T1, T2> : ITuple {
	public T1 Item1;
	public T2 Item2;
	public object this[int index] => index == 0 ? Item1 : Item2;

	public int Length => 2;

	public void Deconstruct(out T1 item1, out T2 item2) {
		item1 = Item1;
		item2 = Item2;
	}
}
