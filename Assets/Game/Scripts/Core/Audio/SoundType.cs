namespace Game.Scripts.Core.Audio
{
    public enum SoundType
    {
        //UI
        UiClick = 0,
        UiHover = 1,
        UiBack = 2,
        UiConfirm = 3,
        PageChange = 4,
        
        PopUpOpen,
        PopUpClose,
        
        //Task
        TaskComplete,
        LevelComplete,
        
        //PickUp
        ItemPickUp,
        
        //Puzzle
        PuzzleButtonPress,
        PuzzleButtonLight,
        PuzzleComplete,
        
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