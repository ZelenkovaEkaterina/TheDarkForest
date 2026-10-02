using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private MenuController menuController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) menuController.Back();
        if (Input.GetKeyDown(KeyCode.P))      menuController.TogglePause();
        if (Input.GetKeyDown(KeyCode.K))      menuController.ToggleShop();
    }
}
