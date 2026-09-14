// PowerUpData.cs
// Defines what a power up IS - its type, sprite and duration

using UnityEngine;

public enum PowerUpType { TimeFreeze, DoubleMultiplier, AutoSort }

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "HeistGame/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public PowerUpType powerUpType;
    public Sprite powerUpSprite;
    public Color powerUpColor = Color.white;
    public float duration = 5f;
}