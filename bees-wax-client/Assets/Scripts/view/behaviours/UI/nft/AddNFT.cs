using api;
using Hexart_UI.Scripts;
using service;
using UnityEngine;
using UnityEngine.UI;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class AddNFT : InjectableBehaviour
    {
        [Inject] private NFTApi _nft;
        [Inject] private WalletService _wallet;
        
        private bool _rotating;
        private float _currentRotation = 0;
        private Button _button;

        private void Start()
        {
            _button = GetComponentInChildren<Button>();
        }

        public async void AddNewNFT()
        {
            _button.interactable = false;
            _button.GetComponent<UIElementSound>().enabled = false;
            GetComponent<HoverHandler>().enabled = false;
            _rotating = true;

            await _nft.AddNFT(_wallet.WalletId);

            _button.interactable = true;
            _button.GetComponent<UIElementSound>().enabled = true;
            GetComponent<HoverHandler>().enabled = true;
            _rotating = false;
        }

        private void Update()
        {
            if (!_rotating)
                return;

            _currentRotation -= 360f * Time.deltaTime;

            _button.transform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);
        }
    }
}