using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using utils;
using utils.injection;
using utils.signal;

namespace api
{
    // public class NestedJsonConverter : JsonConverter
    // {
    //     public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //     {
    //         serializer.Serialize(writer, value);
    //     }
    //
    //     public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
    //         JsonSerializer serializer)
    //     {
    //         return null;
    //         try
    //         {
    //             return reader.Value is string stringObj
    //                 ? JsonConvert.DeserializeObject(Cleanup(stringObj), objectType)
    //                 : serializer.Deserialize(reader, objectType);
    //         }
    //         catch (Exception e)
    //         {
    //             Debug.LogWarning(e.Message);
    //         }
    //
    //         return null;
    //     }
    //
    //     public override bool CanConvert(Type objectType)
    //     {
    //         return true;
    //     }
    //
    //     private string Cleanup(string value)
    //     {
    //         value = value.Replace("\\", "");
    //         value = value.Replace("\"{", "{");
    //         value = value.Replace("}\"", "}");
    //         Debug.Log(value);
    //         return value;
    //     }
    // }


    [Serializable]
    public struct Token
    {
        [JsonProperty(PropertyName = "token_id")]
        public string Id;

        [JsonProperty(PropertyName = "metadata")]
        public NFT Metadata;
    }

    public struct NFT
    {
        public string Id;

        [JsonProperty(PropertyName = "title")] public string Title;

        [JsonProperty(PropertyName = "description")]
        public string Description;

        [JsonProperty(PropertyName = "media")] public string Media;

        // [JsonConverter(typeof(NestedJsonConverter))]
        [JsonProperty(PropertyName = "extra")] public GameplayEffect Extra;
    }

    public class GameplayEffect
    {
        public float harvest = 1;
        public float feed = 1;
        public float damage = 1;
        public float armour = 1;
        public float build = 1;
        public float hp = 1;
        public float hatch = 1;

        public void Merge(GameplayEffect other)
        {
            harvest *= other.harvest;
            damage *= other.damage;
            armour *= other.armour;
            build *= other.build;
            hp *= other.hp;
            hatch *= other.hatch;
        }
    }

    [Singleton]
    public class NFTApi
    {
        private const string ApiNftBaseURL = "http://localhost:8084/api/nft";
        private List<NFT> _owned;
        
        public Signal updated = new Signal();

        public async Task<List<NFT>> GetNFTs(string walletId)
        {
            if (_owned == null)
            {
                var request = UnityWebRequest.Get(ApiNftBaseURL + "?id=" + walletId);
                await request.SendWebRequest();

                _owned = JsonConvert.DeserializeObject<Token[]>(Cleanup(request.downloadHandler.text)).Select(t =>
                {
                    var result = t.Metadata;
                    result.Id = t.Id;
                    return result;
                }).Where(e=>e.Extra!=null).ToList();
            }

            return _owned;
        }
        private string Cleanup(string value)
        {
            value = value.Replace("\\", "");
            value = value.Replace("\"{", "{");
            value = value.Replace("}\"", "}");
            return value;
        }

        public async Task<NFT> AddNFT(string walletId)
        {
            var json = JsonConvert.SerializeObject(new {id =walletId});
            var request = UnityWebRequest.Put(ApiNftBaseURL, Encoding.UTF8.GetBytes(json));

            request.method = UnityWebRequest.kHttpVerbPOST;

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest();
            var token = JsonConvert.DeserializeObject<Token>(Cleanup(request.downloadHandler.text));
            var newNFT = token.Metadata;

            newNFT.Id = token.Id;
        
            _owned.Add(newNFT);
            updated.Dispatch();
        
            return newNFT;
        }
    }
}