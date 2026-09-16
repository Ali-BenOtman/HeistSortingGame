// ObstacleData.cs
// Defines what an obstacle IS - its type, sprite and behavior

using UnityEngine;

public enum ObstacleType { Fake }

[CreateAssetMenu(fileName = "NewObstacle", menuName = "HeistGame/Obstacle")]
public class ObstacleData : ScriptableObject
{
    public ObstacleType obstacleType;
    public Sprite obstacleSprite;
    public Color obstacleColor = Color.white;
}