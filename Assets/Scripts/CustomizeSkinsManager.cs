using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomizeSkinsManager
{
    public static CustomizeElementSkin[] _cellSkins = new CustomizeElementSkin[] { new CustomizeElementSkin(0, "Standard Seat"), new CustomizeElementSkin(0,""), new CustomizeElementSkin(0, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(3, "Valentine's Seat") };

    public static CustomizeElementSkin[] _expositorSkins = new CustomizeElementSkin[] { new CustomizeElementSkin(0, "Standard Seat"), new CustomizeElementSkin(0, ""), new CustomizeElementSkin(0, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(3, "Valentine's Seat") };

    public static CustomizeElementSkin[] _groundSkins = new CustomizeElementSkin[] { new CustomizeElementSkin(0, "Standard Seat"), new CustomizeElementSkin(0, ""), new CustomizeElementSkin(0, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(1, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(2, ""), new CustomizeElementSkin(3, "Valentine's Seat") };
}
public class CustomizeElementSkin
{
    public int _rarity;
    public string _name;
    public CustomizeElementSkin(int rarity, string name)
    {
        _rarity = rarity;
        _name = name;
    }
}