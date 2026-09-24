using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform backSocket;
    [SerializeField] private Transform handSocket;
    
    [SerializeField] private GameObject bow;

    private bool isInHand = false;

    void Start()
    {
        EquipToBack();
    }

    public void EquipToBack()
    {
        AttachTo(backSocket);
        isInHand = false;
    }

    public void EquipToHand()
    {
        AttachTo(handSocket);
        isInHand = true;
    }

    public void Toggle()
    {
        if (isInHand) EquipToBack();
        else EquipToHand();
    }

    private void AttachTo(Transform socket)
    {
        bow.transform.SetParent(socket);
        bow.transform.localPosition = Vector3.zero;
        bow.transform.localRotation = Quaternion.identity;
    }
}
