using System;
using System.Collections.Generic;
using KyleDulce.SocketIo;
using model;
using model.data;
using Newtonsoft.Json;
using UnityEngine;
using utils.injection;
using utils.signal;

namespace api
{
    [Serializable]
    internal struct ResourceData
    {
        public Dictionary<string, float> players;
        public float remainingResource;
    }
    
    [Singleton]
    public class EscrowApi
    {
        [Inject] private ResourceModel _resources;
        
        [Inject] private NakamaApi _nakamaApi;

        public readonly Signal MatchEnd = new Signal();
        
        Socket _socket;
        
        public void Connect()
        {
            _socket = SocketIo.establishSocketConnection("ws://localhost:8083");
            _socket.connect();
            _socket.on("start", OnStart);
            _socket.on("res", OnResourceUpdated);
            _socket.on("end", OnEnd);
        }
        
        public void Start(int tier)
        {
            _socket.emit("start", JsonConvert.SerializeObject(new {matchId = _nakamaApi.MatchmakerResult?.MatchId ??"test", tier}));
        }

        public void SubmitResource(float value)
        {
            _socket.emit("collection", value.ToString());
        }

        void OnStart(string value) {
            _resources.remaining = _resources.total = float.Parse(value);
        }
        
        void OnResourceUpdated(string value)
        {
            var data = JsonConvert.DeserializeObject<ResourceData>(value);

            _resources.remaining = data.remainingResource;
            
            foreach (var key in data.players.Keys)
                _resources.Set(key == _socket.connectionId? Faction.Player:Faction.Enemy, data.players[key]);
        }
        
        void OnEnd(string value) {
            MatchEnd.Dispatch();
        }
    }
}

