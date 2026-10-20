using UnityEngine;

public class Package : MonoBehaviour
{
    public void PickUp()
    {
        GameManager.Instance.PackagePickedUp();
        gameObject.SetActive(false);
    }
}