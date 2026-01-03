using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    public void SetSelected(bool isSelected)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(isSelected);
    }
    public void SetHovered(bool isHovered)
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(isHovered);
    }
}
