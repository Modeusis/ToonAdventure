using UnityEngine;

namespace Game.Scripts.Utilities.MonoDebugs
{
    public class EventDebug : MonoBehaviour
    {
        public void MethodA(string text)
        {
            Debug.Log($"[EventDebug.MethodA] - {text}");
        }
        
        public void MethodB(string text)
        {
            Debug.Log($"[EventDebug.MethodB] - {text}");
        }
        
        public void MethodC(string text)
        {
            Debug.Log($"[EventDebug.MethodC] - {text}");
        }
    }
}