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

    [Header("Scoring")]
    [Tooltip("Points awarded for sorting this bill correctly - deliberately " +
             "decoupled from the displayed dollar value above. Bill art still " +
             "shows real denominations ($1-$100), but actual score stays " +
             "compressed and skill-driven rather than swung by which bill spawns.")]
    public int scorePoints = 1;
}