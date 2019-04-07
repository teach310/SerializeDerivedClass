using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person
{
    public string name;
    public int age;
}

[System.Serializable]
public class Man : Person
{
    public string manField;
}

[System.Serializable]
public class Woman : Person
{
    public int womanField;
}