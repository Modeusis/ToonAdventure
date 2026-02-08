namespace Game.Scripts.Utilities.Events
{
    public struct OnQuestProgressEvent
    {
        public string TargetId;
        public int Amount;
    }

    public struct OnQuestCompletedEvent
    {
        public string QuestId;
    }
    
    public struct OnQuestStepCompletedEvent
    {
        public string StepDescription;
    }
}