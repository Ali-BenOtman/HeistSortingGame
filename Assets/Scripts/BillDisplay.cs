// BillDisplay.cs
// Controls how the bill looks on screen - its color and value text

using UnityEngine;
using TMPro;

public class BillDisplay : MonoBehaviour
{
    [Header("Visual Components")]
    public SpriteRenderer billSprite;
    public TextMeshProUGUI valueText;

    public void SetupBill(BillData data)
    {
        if (billSprite != null && data.billSprite != null)
            billSprite.sprite = data.billSprite;

        if (billSprite != null)
            billSprite.color = data.billColor;

        if (valueText != null)
            valueText.text = "$" + (int)data.value;
    }

    public void SetupObstacle(ObstacleData data)
    {
        if (billSprite != null && data.obstacleSprite != null)
            billSprite.sprite = data.obstacleSprite;

        if (billSprite != null)
            billSprite.color = data.obstacleColor;

        if (valueText != null)
            valueText.text = "";
    }
    public void SetupPowerUp(PowerUpData data)
{
    if (billSprite != null && data.powerUpSprite != null)
        billSprite.sprite = data.powerUpSprite;

    if (billSprite != null)
        billSprite.color = data.powerUpColor;

    if (valueText != null)
        valueText.text = "";
}
}