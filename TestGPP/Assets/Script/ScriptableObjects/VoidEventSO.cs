using UnityEngine;
using UnityEngine.Events;

namespace Game.Stack.Core
{
    [CreateAssetMenu(fileName = "Void Event", menuName = "ScriptableObjects/Events", order = 1)]
    public class VoidEventSO : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaisedEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
