namespace Game.Scripts.Core.Audio
{
    public enum SoundType
    {
        //UI
        UiClick = 0,
        
        //Task
        TaskComplete = 35,
        LevelComplete = 36,
        
        //PickUp
        ItemPickUp,
        
        //Puzzle
        PuzzleButtonPress = 50,
        PuzzleButtonLight = 51,
        PuzzleComplete = 52,
        
        //Doors
        DoorOpen = 10,
        DoorClose = 11,
        FridgeOpen = 12,
        FridgeClose = 13,
        
        //Steps
        StepWood = 20,
        StepTile = 21,
        StepGrass = 22,
        
        CharacterDialogue = 23,
        
        //Dog
        DogBark = 40,
        DogWhine = 41,
        DogStep = 42
    }
}