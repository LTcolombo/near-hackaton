using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using utils.injection;
using utils;
using utils.signal;

namespace service
{
    [Singleton]
    public class WalletService
    {
        public Signal Update = new Signal();

        public string WalletId;
        private int? _balance;

        private const string ApiBalanceBaseURL = "http://localhost:8084/api/balance";

        public async Task<int> GetBalance()
        {
            if (_balance == null)
            {
                var request = UnityWebRequest.Get(ApiBalanceBaseURL + "?id=" + WalletId);
                await request.SendWebRequest();
                _balance = int.Parse(request.downloadHandler.text);
            }

            return _balance ?? 0;
        }

        public async Task<bool> Withdraw(int value)
        {
            var json = JsonConvert.SerializeObject(new
            {
                id =WalletId,
                value
            });

            var request = UnityWebRequest.Put(ApiBalanceBaseURL, Encoding.UTF8.GetBytes(json));

            request.method = UnityWebRequest.kHttpVerbPOST;

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest();

            _balance = null; //dirty
            Update.Dispatch();

            return request.result == UnityWebRequest.Result.Success;
        }
    }
}