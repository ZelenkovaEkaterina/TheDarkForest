using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuScreen 
{ 
    None, 
    Main, 
    Pause, 
    SkillShop,
    Restart
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _skillShop;
    [SerializeField] private GameObject _restartGame;
    
    private MenuScreen _current = MenuScreen.None;
    private MenuScreen _shopReturn = MenuScreen.Pause;

    private void Start() => Open(MenuScreen.Main);

    public MenuScreen Current => _current;

    private void OnEnable()
    {
        _gameController.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        _gameController.OnGameOver -= GameOver;
    }

    public void StartGame()
    {
        if (_current != MenuScreen.Main)
            return;
        
        Open(MenuScreen.None);
    }

    public void BackToMenu()
    {
        if (_current != MenuScreen.Pause && _current != MenuScreen.Restart)
            return;
        
        Open(MenuScreen.Main);
    }

    public void Open(MenuScreen screen)
    {
        if (screen == MenuScreen.SkillShop)
            _shopReturn = _current;

        _current = screen;

        _mainMenu.SetActive(screen == MenuScreen.Main);
        _pauseMenu.SetActive(screen == MenuScreen.Pause);
        _skillShop.SetActive(screen == MenuScreen.SkillShop);
        _restartGame.SetActive(screen == MenuScreen.Restart);

        Time.timeScale = (screen == MenuScreen.None) ? 1f : 0f;
    }

    public void Back()
    {
        switch (_current)
        {
            case MenuScreen.SkillShop: Open(_shopReturn); break;
            case MenuScreen.Pause:     Open(MenuScreen.None); break;
            case MenuScreen.None:      Open(MenuScreen.Pause); break;
        }
    }

    public void TogglePause()
    {
        if (_current == MenuScreen.Pause) Open(MenuScreen.None);
        else if (_current == MenuScreen.None) Open(MenuScreen.Pause);
    }
    
    public void ToggleShop()
    {
        if (_current == MenuScreen.Main || _current == MenuScreen.Pause) return;
        
        if (_current == MenuScreen.SkillShop) Back();
        else Open(MenuScreen.SkillShop);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Open(MenuScreen.Restart);
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
