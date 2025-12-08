using Godot;
using System;
using System.Collections.Generic;

namespace TrucoProject.Net.Events
{
    public static class NetEventBus
    {
        // Diccionario:
        // Evento → lista de callbacks
        private static readonly Dictionary<NetEvent.Type, List<Action<NetEvent>>> _listeners
            = new();

        // ---------------------------
        // SUBSCRIBE
        // ---------------------------
        public static void Subscribe(NetEvent.Type eventType, Action<NetEvent> callback)
        {
            if (!_listeners.ContainsKey(eventType))
                _listeners[eventType] = new List<Action<NetEvent>>();

            _listeners[eventType].Add(callback);
        }

        // ---------------------------
        // UNSUBSCRIBE
        // ---------------------------
        public static void Unsubscribe(NetEvent.Type eventType, Action<NetEvent> callback)
        {
            if (!_listeners.ContainsKey(eventType))
                return;

            _listeners[eventType].Remove(callback);
        }

        // ---------------------------
        // EMIT  (🔥 esto faltaba)
        // ---------------------------
        public static void Emit(NetEvent netEvent)
        {
            if (netEvent == null)
                return;

            if (!_listeners.ContainsKey(netEvent.EventType))
                return;

            // Copiamos la lista por seguridad
            var targets = new List<Action<NetEvent>>(_listeners[netEvent.EventType]);

            foreach (var callback in targets)
            {
                try
                {
                    callback(netEvent);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[NetEventBus] Handler error: {ex.Message}");
                }
            }
        }
    }
}
