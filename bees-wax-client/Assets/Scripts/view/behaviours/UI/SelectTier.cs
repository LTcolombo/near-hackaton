using model;
using UnityEngine.SceneManagement;
using utils.injection;

namespace view.behaviours.UI
{
    public class SelectTier : InjectableBehaviour
    {
        [Inject] private SelectedTierModel _tier;

        public void Select(int value)
        {
            _tier.value = value;
            SceneManager.LoadScene("Lobby");
        }
    }
}
