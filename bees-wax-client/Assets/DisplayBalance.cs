using service;
using UnityEngine.UI;
using utils.injection;

public class DisplayBalance : InjectableBehaviour
{
    [Inject]
    private WalletService _wallet;
    
    public Text text;
    
    void Start()
    {
        Refresh();
        _wallet.Update.Add(Refresh);
    }

    async void Refresh()
    {
        text.text = await _wallet.GetBalance() + " yN";
    }
}
