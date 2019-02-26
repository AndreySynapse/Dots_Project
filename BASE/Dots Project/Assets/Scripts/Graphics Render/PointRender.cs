using UnityEngine;

public class PointRender : MonoBehaviour
{
    [SerializeField] private Transform _pointPrefab;

    public Transform GetPoint()
    {
        return Instantiate(_pointPrefab);
    }
}
