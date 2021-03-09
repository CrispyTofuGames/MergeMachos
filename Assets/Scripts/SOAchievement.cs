using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "ScriptableObject/Achievement", order = 2)]
public class SOAchievement : ScriptableObject
{
    public int dinoLevel;
    public int amount;
    public int rewardAmount;
}