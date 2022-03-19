using System;
using System.Linq;
using api;
using service;
using UnityEngine;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class DisplaySelectedNFT : InjectableBehaviour
    {
        public Transform container;
        public DisplayNFT nftRenderer;

        [Inject] private NFTApi _nft;
        [Inject] private NFTSelectionService _selectionService;
        [Inject] private WalletService _wallet;

        private void Start()
        {
            Redraw();
            _selectionService.Updated.Add(Redraw);
        }

        private async void Redraw()
        {
            var selection = await _selectionService.GetSelection();

            if (container == null)
                return;
            
            foreach (Transform child in container)
                Destroy(child.gameObject);

            var owned = await _nft.GetNFTs(_wallet.WalletId);
        
            foreach (var nft in selection)
            {
                var newNftRenderer = Instantiate(nftRenderer.gameObject, container);
                newNftRenderer.GetComponent<DisplayNFT>().SetData(owned.FirstOrDefault(e => e.Id == nft));
            }
        }

        private void OnDestroy()
        {
            _selectionService.Updated.Remove(Redraw);
        }
    }
}