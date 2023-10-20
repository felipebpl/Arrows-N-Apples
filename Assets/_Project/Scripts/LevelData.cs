using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData", fileName = "NewLevelData")]
public class LevelData : ScriptableObject
{
    public Sprite BackgroundTexture;
    public float Distance;
    public FruitType FruitType;
    public bool IsRopeLevel;
}
