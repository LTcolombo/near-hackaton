using Hexart_UI.Scripts;
using service;
using UnityEngine;
using UnityEngine.UI;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class ConfirmWithdrawal : InjectableBehaviour
    {
        [Inject] private WalletService _wallet;
        
        public Button button;
        public Text text;
        
        private bool _rotating;
        private float _currentRotation = 0;


        public async void WithdrawConfirmed()
        {
            button.interactable = false;
            button.GetComponent<UIElementSound>().enabled = false;
//            GetComponent<HoverHandler>().enabled = false;
            _rotating = true;

            await _wallet.Withdraw(int.Parse(text.text));
            text.text = "";

            button.interactable = true;
            button.GetComponent<UIElementSound>().enabled = true;
       //     GetComponent<HoverHandler>().enabled = true;
            _rotating = false;
        }

        private void Update()
        {
            if (!_rotating)
                return;

            _currentRotation -= 360f * Time.deltaTime;

            button.transform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);
        }
    }
}