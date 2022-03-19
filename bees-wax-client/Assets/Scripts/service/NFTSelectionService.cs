using System;
using System.Linq;
using System.Threading.Tasks;
using api;
using utils.injection;
using utils.signal;

namespace service
{
    [Singleton]
    public class NFTSelectionService
    {
        public Signal Updated = new Signal();
        public Signal Selected = new Signal();
        
        [Inject] private NakamaApi _nakama;
        
        [Inject] private NFTApi _nft;
        
        [Inject] private WalletService _wallet;

        public async Task<string[]> GetSelection()
        {
            if (_buffer != null)
                return _buffer;

            await Init();
            return _buffer;
        }

        private string[] _selection;
        private string[] _buffer;
        
        public GameplayEffect Effect { get; private set; }

        private async Task Init()
        {
            _selection = new string[3];
            var storedSelection = await _nakama.GetSelectedNFT();
            if (storedSelection!=null)
                for (var i = 0; i < Math.Min(3, storedSelection.Length); i++)
                {
                    _selection[i] = storedSelection[i];
                }

            Reset();
        }

        public bool TrySetState(string dataId, bool value)
        {
            if (!value)
            {
                for (var i = 0; i < _buffer.Length; i++)
                    if (_buffer[i] == dataId){
                        _buffer[i] = null;
                        break;
                    }
                CalculateEffect();
                Selected.Dispatch();
                return true;
            }
            else
            {
                for (var i = 0; i < _buffer.Length; i++)
                    if (_buffer[i] == null){
                        _buffer[i] = dataId;
                        CalculateEffect();
                        Selected.Dispatch();
                        return true;
                    }
                return false;
            }
        }

        public async Task Apply()
        {
            await _nakama.SaveSelectedNFT(_buffer);
            _selection = (string[]) _buffer.Clone();
            Updated.Dispatch();
        }

        public void Reset()
        {
            _buffer = (string[]) _selection.Clone();
            CalculateEffect();
        }

        private async void CalculateEffect()
        {
            var owned = await _nft.GetNFTs(_wallet.WalletId);

            Effect = new GameplayEffect();

            foreach (var id in await GetSelection())
            {
                if (id == null) 
                    continue;
                
                if (!owned.Exists(e => e.Id == id))
                    continue;
            
                var nft = owned.FirstOrDefault(e => e.Id == id);
                Effect.Merge(nft.Extra);
            }
        }
    }
}