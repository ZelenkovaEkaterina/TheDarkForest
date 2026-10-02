using UnityEngine;

public enum MenuScreen 
{ 
    None, 
    Main, 
    Pause, 
    SkillShop 
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject skillShop;
    
    private MenuScreen _current = MenuScreen.None;
    private MenuScreen _shopReturn = MenuScreen.Pause;

    private void Start() => Open(MenuScreen.Main);

    public MenuScreen Current => _current;

    public void StartGame()
    {
        if (_current != MenuScreen.Main)
            return;
        
        _current = MenuScreen.None;
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        if (_current != MenuScreen.Pause)
            return;
        
        _current = MenuScreen.Main;
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Open(MenuScreen screen)
    {
        if (screen == MenuScreen.SkillShop)
            _shopReturn = _current;

        _current = screen;

        mainMenu.SetActive(screen == MenuScreen.Main);
        pauseMenu.SetActive(screen == MenuScreen.Pause);
        skillShop.SetActive(screen == MenuScreen.SkillShop);

        Time.timeScale = (screen == MenuScreen.None) ? 1f : 0f;
    }

    public void Back()
    {
        switch (_current)
        {
            case MenuScreen.SkillShop: Open(_shopReturn); break;
            case MenuScreen.Pause:     Open(MenuScreen.None); break;
        }
    }

    public void TogglePause()
    {
        if (_current == MenuScreen.Pause) Open(MenuScreen.None);
        else if (_current == MenuScreen.None) Open(MenuScreen.Pause);
    }
    
    public void ToggleShop()
    {
        if (_current == MenuScreen.SkillShop) Back();
        else Open(MenuScreen.SkillShop);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
