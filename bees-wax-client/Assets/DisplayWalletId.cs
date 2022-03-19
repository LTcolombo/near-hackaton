using service;
using UnityEngine.UI;
using utils.injection;

public class DisplayWalletId : InjectableBehaviour
{
    
    [Inject]
    private WalletService _wallet;
    
    public Text text;
    void Start()
    {
        text.text = _wallet.WalletId;
    }
}
