using System.Linq;
using api;
using service;
using UnityEngine;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class DisplayOwnedNFT : InjectableBehaviour
    {
        public Transform container;
        public DisplayNFT nftRenderer;

        [Inject] private NFTApi _nft;
        [Inject] private NFTSelectionService _selection;
        [Inject] private WalletService _wallet;

        public Transform addBtn;

        private void Start()
        {
            _nft.updated.Add(Redraw);
            Redraw();
        }

        private async void Redraw()
        {
            var owned = await _nft.GetNFTs(_wallet.WalletId);
        
            foreach (Transform child in container)
                if (child.GetComponent<DisplayNFT>() != null)
                    Destroy(child.gameObject);
            
            var selection = await _selection.GetSelection();

            foreach (var nft in owned)
            {
                var newRenderer = Instantiate(nftRenderer.gameObject, container);
                newRenderer.GetComponent<DisplayNFT>()?.SetData(nft);
                newRenderer.GetComponent<SelectHandler>()?.SetSelected(selection.ToList().Contains(nft.Id));
            }
        
            addBtn.SetSiblingIndex(container.childCount-1);
        }
    }
}