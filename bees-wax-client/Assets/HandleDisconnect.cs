using service;
using utils.injection;
using UnityEngine.SceneManagement;

public class HandleDisconnect : InjectableBehaviour
{
    [Inject]
    private MultiplayerService _multiplayer;
    
    private bool _disconnect;

    // Start is called before the first frame update
    void Start()
    {
        _multiplayer.UsersUpdated.Add(OnUsersUpdated);
        _multiplayer.Disconnect.Add(OnDisconnect);
    }

    private void OnUsersUpdated()
    {
        if (_multiplayer.Users.Count < 2)
            _disconnect = true;
    }


    private void OnDisconnect()
    {
        _disconnect = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_disconnect)
            SceneManager.LoadScene("Loading");
    }

    private void OnDestroy()
    {
        _multiplayer.UsersUpdated.Remove(OnUsersUpdated);
        _multiplayer.Disconnect.Remove(OnDisconnect);
    }
}