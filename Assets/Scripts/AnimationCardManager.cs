using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCardManager
{
    public static AnimationCard[] _animationCards = 
        new AnimationCard[]{new AnimationCard(15,0, "Maddie"), new AnimationCard(4, 0, "Shyva"),
        new AnimationCard(3, 1, "Suky")};

}
public class AnimationCard
{
    public int _character;
    public int _rarity;
    public string _name;
    public AnimationCard(int character, int rarity, string name) //Rarity 0 va a ser gratis
    {
        _character = character;
        _rarity = rarity;
        _name = name;
    }
}