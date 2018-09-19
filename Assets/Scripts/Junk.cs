using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Junk", menuName = "New Junk Item")]
public class Junk : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public AudioClip sound;

    [Header("Stat modifiers")]
    [Tooltip("increases or decreases player damage to walls")]
    [Range(-1, 1)]
    public int attack;

    [Tooltip("increases or decreases food lost from enemy damage")]
    [Range(-1, 1)]
    public int defense;

    [Tooltip("increases or decreases food added when picking up food item")]
    [Range(-1, 1)]
    public int foodPerPickup;

    [Tooltip("increases or decreases food used per movement action")]
    [Range(-1, 1)]
    public int foodPerMove;

    [Header("World modifiers")]
    [Tooltip("adds or subtracts a column from the level")]
    [Range(-1, 1)]
    public int column;

    [Tooltip("adds or subtracts a row from the level")]
    [Range(-1, 1)]
    public int row;

    [Tooltip("adds or subtracts an enemy from the level")]
    [Range(-1, 1)]
    public int enemy;

    [Tooltip("adds or subtracts a food item from the level")]
    [Range(-1, 1)]
    public int foodPerLevel;

}