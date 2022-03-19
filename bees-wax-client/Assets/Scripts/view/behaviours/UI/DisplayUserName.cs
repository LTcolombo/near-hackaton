using service;
using UnityEngine.UI;
using utils.injection;

namespace view.behaviours.UI
{
    public class DisplayUserName : InjectableBehaviour
    {
        [Inject]
        MultiplayerService _multiplayer;
        
        [Inject]
        WalletService _wallet;
    
        void OnEnable()
        {
            if (_multiplayer.UserName == null)
                _multiplayer.SetUserName(_wallet.WalletId);
            
            _multiplayer?.UserUpdated?.Add(OnUserUpdated);
            OnUserUpdated();
        }

        private void OnUserUpdated()
        {
            GetComponent<Text>().text = (_multiplayer.UserName ?? _wallet.WalletId).ToUpper();
        }

        private void OnDisable()
        { 
            _multiplayer?.UserUpdated?.Remove(OnUserUpdated);
        }
    }
}