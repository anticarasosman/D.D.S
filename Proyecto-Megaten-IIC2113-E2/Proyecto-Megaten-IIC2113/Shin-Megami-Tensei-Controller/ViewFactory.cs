using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei;

public class ViewFactory
{
    View _view;
    public ViewFactory(View view)
    {
        _view = view;
    }
    public ViewStartOfGame CreateViewStartOfGame()
    {
        return new ViewStartOfGame(_view);
    }
    public ActionView CreateActionView()
    {
        return new ActionView(_view);
    }
    public GameView CreateGameView()
    {
        return new GameView(_view);
    }
}