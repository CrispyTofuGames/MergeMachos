using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SpecialSkinsManager 
{
    public static SpecialSkin[] _specialSkins  = new SpecialSkin[]{new SpecialSkin(4,0, "Christmas Shiva"), new SpecialSkin(3,0, "Christmas Suky"), new SpecialSkin(5,1, "Christmas Danny"), new SpecialSkin(11,2, "New Year Mury"), new SpecialSkin(2,3, "Carnival Cuticorn"), new SpecialSkin(0, 2, "Romantic Takoyaki"), new SpecialSkin(11, 2, "Romantic Takoyaki")};
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
