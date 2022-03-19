using service;
using UnityEngine.UI;
using utils.injection;

namespace view.behaviours.UI
{
    public class SetUserName : InjectableBehaviour
    {
        public Text Source;

        [Inject]
        private MultiplayerService _multiplayer;
    
    
        public void Apply()
        {
            _multiplayer.SetUserName(Source.text);
        }

    }
}
