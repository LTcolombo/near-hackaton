using service;
using utils.injection;

namespace view.behaviours.UI
{
    public class JoinMatch : InjectableBehaviour
    {
        [Inject]
        private MultiplayerService _multiplayer;

        public async void OnClick()
        {
            await _multiplayer.Ready();
        }

    }
}
