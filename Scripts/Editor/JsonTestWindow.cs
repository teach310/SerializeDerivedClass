using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JsonTestWindow : EditorWindow {
    [MenuItem ("JsonTest/JsonTestWindow")]
    static void Open () {
        GetWindow<JsonTestWindow> ();
    }

    void OnGUI () {
        if (GUILayout.Button ("Failed")) {
            TestPersonFailed ();
        }

        if (GUILayout.Button ("OK")) {
            TestPersonListOK ();
        }
        if (GUILayout.Button ("Create PlayerList")) {
            new DerivedClassListGenerator().CreateAt(typeof(Person));
        }
    }

    void TestPersonListOK () {
        var target = new List<Person> () {
            new Man { age = 18, name = "男性", manField = "man" },
            new Woman { age = 16, name = "女性", womanField = 100 }
        };
        var personList = new PersonList (target);

        // Object -> string
        var json = JsonUtility.ToJson (personList);
        Debug.Log (json);

        // string -> Object
        var personList2 = JsonUtility.FromJson<PersonList> (json);
        var target2 = personList2.Convert ();
        var man = target2[0] as Man;
        Debug.Log ($"age={man.age},name={man.name}, manField={man.manField}");
        var woman = target2[1] as Woman;
        Debug.Log ($"age={woman.age},name={woman.name}, manField={woman.womanField}");
    }

    [System.Serializable]
    public class TestPersonList{
        public List<Person> personList = new List<Person>();
    }

    void TestPersonFailed () {
        var target = new List<Person> () {
            new Man { age = 18, name = "男性", manField = "man" },
            new Woman { age = 16, name = "女性", womanField = 100 }
        };
        var testPersonList = new TestPersonList(){personList = target};
        var json = JsonUtility.ToJson (testPersonList); // manField, womanFieldが消える
        Debug.Log (json);
    }
}