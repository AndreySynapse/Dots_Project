public class MainMenuView : BaseView
{
    #region Events
    public void OnPlayButtonClick()
    {
        _gameManager.MakeTransition(ViewKinds.Game);
    }
    #endregion
}
