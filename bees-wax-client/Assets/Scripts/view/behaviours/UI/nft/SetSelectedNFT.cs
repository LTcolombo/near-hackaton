using service;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class SetSelectedNFT : InjectableBehaviour
    {
        [Inject] 
        private NFTSelectionService _selectedNFTs;

        public void Apply()
        {
            _selectedNFTs.Apply();
        }
        
        public void Cancel()
        {
            _selectedNFTs.Reset();
        }

    }
}
