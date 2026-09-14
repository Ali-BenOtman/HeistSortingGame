// BillData.cs
// Defines what a bill IS - its value and which bag it belongs to

using UnityEngine;

public enum BillValue { One = 1, Five = 5, Twenty = 20, Hundred = 100 }
public enum SwipeDirection { Up, Down, Left, Right }

[CreateAssetMenu(fileName = "NewBill", menuName = "HeistGame/Bill")]
public class BillData : ScriptableObject
{
    public BillValue value;
    public SwipeDirection correctDirection;
    public Color billColor;
    public Sprite billSprite;
}