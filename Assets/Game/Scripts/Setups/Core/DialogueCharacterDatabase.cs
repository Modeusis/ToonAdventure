using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New characters database", menuName = "Setup/Core/Dialogue database")]
    public class DialogueCharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterData> _characters;
        
        public IReadOnlyList<CharacterData> Characters => _characters;

        public bool TryGetCharacter(string id, out CharacterData data)
        {
            foreach (var character in _characters)
            {
                if (character.Id == id)
                {
                    data = character;
                    return true;
                }
            }
            data = null;
            return false;
        }
    }
    
    [Serializable]
    public class CharacterData
    {
        [field: SerializeField] public string Id {get; private set; }
        [field: SerializeField] public string DisplayName {get; private set; }
        [field: SerializeField] public Sprite DefaultPortrait {get; private set; }
    }
}