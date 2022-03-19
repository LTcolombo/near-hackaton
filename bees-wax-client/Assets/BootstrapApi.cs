using System.Runtime.InteropServices;
using api;
using service;
using utils.injection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootstrapApi : InjectableBehaviour
{
    [Inject] private NakamaApi _nakama;

    [Inject] private MultiplayerService _multiplayer;

    [Inject] private NFTSelectionService _selection;

    [Inject] private WalletService _wallet;

    [Inject] private EscrowApi _escrow;

    public Text Status;


    [DllImport("__Internal")]
    private static extern void SignIn();

    private void Start()
    {
#if UNITY_EDITOR
        HandleWalletId("beeswax_client_1.testnet");
#elif UNITY_WEBGL
//this should do into wallet
        SignIn();
#else
        HandleWalletId("beeswax_client_2.testnet");
#endif
        
        _escrow.Connect();
    }

    public void HandleWalletId(string value)
    {
        _wallet.WalletId = value;
        
        UnityMainThreadDispatcher.Instance().Enqueue(Connect);
    }

    private async void Connect()
    {
        Status.text = "Startup..";
        _nakama.CreateClient();

        Status.text = "Authenticating..";
        await _nakama.CreateSession(_wallet.WalletId);
        
        Status.text = "Getting user..";
        await _nakama.GetUser();
        
        Status.text = "Creating socket..";
        await _nakama.CreateSocket();
        
        //possibly thsi should be in _multiplayer Awake()? todo
        //todo make resolution recursive
        Injector.Instance.Resolve(_multiplayer);
        Injector.Instance.Resolve(_selection);
        Injector.Instance.Resolve(_escrow);
        
        _multiplayer.InitApi();
        
        SceneManager.LoadScene("TierSelection");
    }
}