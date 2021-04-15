using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SpecialSkinsManager 
{
    public static SpecialSkin[] _specialSkins/*  = new SpecialSkin[]{new SpecialSkin(4,0, "Christmas Shiva")}*/;
}
public class SpecialSkin
{
    public int _character;
    public int _rarity;
    public string _name;
    public SpecialSkin( int character, int rarity, string name)
    {
        _character = character;
        _rarity = rarity;
        _name = name;
    }
}
