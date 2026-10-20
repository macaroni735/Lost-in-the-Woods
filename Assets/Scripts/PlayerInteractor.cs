using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRadius = 2.5f;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        foreach (var hit in Physics.OverlapSphere(transform.position, interactRadius))
        {
            if (!GameManager.Instance.HasPackage && hit.TryGetComponent(out Package p))
            {
                p.PickUp();
                return;
            }
            if (GameManager.Instance.HasPackage && hit.TryGetComponent(out DeliveryZone z))
            {
                if (z.TryDeliver()) return;
            }
        }
    }
}