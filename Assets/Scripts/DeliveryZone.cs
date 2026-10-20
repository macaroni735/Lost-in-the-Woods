using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public bool TryDeliver()
    {
        if (!GameManager.Instance.HasPackage) return false;
        GameManager.Instance.DeliveryCompleted();
        return true;
    }
}