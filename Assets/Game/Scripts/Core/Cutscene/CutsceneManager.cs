using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Loop.Dialogues;
using UnityEngine;

namespace Game.Scripts.Core.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private Dialogue _cutsceneDialogue;

        [Header("===Debug===")]
        [SerializeField] private float _cutsceneDurationDebug = 2f;
        
        public async UniTask StartCutscene(Action cutsceneCallback = null)
        {
            Debug.Log("Starting Cutscene");
            
            await UniTask.Delay(TimeSpan.FromSeconds(_cutsceneDurationDebug));
                
            cutsceneCallback?.Invoke();
        }
    }
}