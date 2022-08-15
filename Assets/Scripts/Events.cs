using UnityEngine;
using UnityEngine.Events;

public class Events 
{
    [System.Serializable] public class EventFadeComplet : UnityEvent<bool> { }
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager> { }
    [System.Serializable] public class EventMobDeath : UnityEvent<MobType, Vector3> { }
    [System.Serializable] public class EventIntegerEvent : UnityEvent<int> { }
}
