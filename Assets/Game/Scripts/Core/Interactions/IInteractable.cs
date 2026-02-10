namespace Game.Scripts.Core.Interactions
{
    public interface IInteractable
    {
        InteractionAnimationType Type { get; }
        void Interact();
    }
}