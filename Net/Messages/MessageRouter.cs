using TrucoProject.Net.Events;
using TrucoProject.Net.Messages;
using System.Collections.Generic;
using TrucoProject.Net.Utils;

public sealed class MessageRouter {

    private readonly Dictionary<string, NetEvent.Type> _routes = new();
    private static MessageRouter _instance { get; set; }

    public static MessageRouter GetInstance() {
        if (_instance == null) {
            _instance = new MessageRouter();            
        }

        return _instance;
    } 
    
    private MessageRouter() {
        NetEventBus.Subscribe(
            NetEvent.Type.MessageReceived,
            OnRawMessage
        );
    }

    public void RegisterProtocol(string protocolKey, NetEvent.Type eventType){
        _routes[protocolKey] = eventType;
    }

    private void OnRawMessage(NetEvent evt) {
        if (evt.Payload is not string raw) return;

        var msg = MessageParser.Parse(raw);
        if (msg == null) return;

        if (!_routes.TryGetValue(msg.Type, out var eventType)) {
            NetLogger.Warn($"[Router] No route for {msg.Type}");
            return;
        }

        NetEventBus.Emit(new NetEvent(eventType, msg));
    }
}
