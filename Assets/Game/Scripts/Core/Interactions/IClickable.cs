namespace Game.Scripts.Core.Interactions
{
    public interface IClickable
    {
        void OnBeginHover();
        void OnEndHover();
        void OnClick();
    }
}