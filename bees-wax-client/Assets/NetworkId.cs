using service;
using utils.injection;

public class NetworkId : InjectableBehaviour
{
    [Inject]
    private MultiplayerService _multiplayer;
    
    public int Id;
    
    // Start is called before the first frame update
    void Start()
    {
        _multiplayer.RegisterObject(gameObject, Id);
        Destroy (this);
    }
}
