using Game.Scripts.Core.Character;

namespace Game.Scripts.Utilities.Events
{
    public struct OnPlayerStateChangeRequest
    {
        public PlayerState NewState;

        public OnPlayerStateChangeRequest(PlayerState newState)
        {
            NewState = newState;
        }
    }
}