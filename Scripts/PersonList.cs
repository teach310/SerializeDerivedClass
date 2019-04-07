using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonList {
	[SerializeField] List<int> manIndexList = new List<int> ();
	[SerializeField] List<Man> manList = new List<Man> ();
	[SerializeField] List<int> womanIndexList = new List<int> ();
	[SerializeField] List<Woman> womanList = new List<Woman> ();

	public PersonList (List<Person> src) {
		for (int i = 0; i < src.Count; i++) {
			switch (src[i]) {
				case Man man:
					manIndexList.Add (i);
					manList.Add (man);
					break;
				case Woman woman:
					womanIndexList.Add (i);
					womanList.Add (woman);
					break;

			}
		}
	}

	public List<Person> Convert () {
		int length =
			manIndexList.Count +
			womanIndexList.Count;
		var rtn = new Person[length];
		for (int i = 0; i < manIndexList.Count; i++) {
			rtn[manIndexList[i]] = manList[i];
		}
		for (int i = 0; i < womanIndexList.Count; i++) {
			rtn[womanIndexList[i]] = womanList[i];
		}
		return new List<Person> (rtn);
	}

	public static List<System.Type> GetAllDerivedTypes () {
		return new List<System.Type> () {
			typeof (Man),
			typeof (Woman)
		};
	}
}