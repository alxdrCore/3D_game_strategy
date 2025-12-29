using UnityEngine;

public class ButtonPlaceBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _building;

    public void PlaceBuilding()
    {
        StateHandler.Instance.SetState(new StateBuildingPlacement());
        Instantiate(_building, Vector3.zero, Quaternion.identity);
    }
}
