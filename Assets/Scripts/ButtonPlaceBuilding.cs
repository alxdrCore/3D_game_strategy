using UnityEngine;

public class ButtonPlaceBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _building;

    public void PlaceBuilding()
    {
        Instantiate(_building, Vector3.zero, Quaternion.identity);
    }
}
