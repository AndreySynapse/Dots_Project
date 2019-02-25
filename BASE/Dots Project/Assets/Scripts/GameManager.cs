using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameStates
    {
        Menu,
        Game
    }

    public GameStates CurrentGameState { get; set; }

    protected override void Init()
    {
        base.Init();
        
        this.CurrentGameState = GameStates.Game;
        var l = Application.systemLanguage;
                
        print(l);

        DontDestroyOnLoad(this);
    }

    public void MakeTransition(BaseView.ViewKinds target)
    {
        switch (target)
        {
            case BaseView.ViewKinds.MainMenu:
                SceneManager.LoadScene("Menu Scene");
                break;

            case BaseView.ViewKinds.Game:
                SceneManager.LoadScene("Game Scene");
                break;
        }
    }
}
