using api;
using model;
using model.data;
using UnityEngine;
using UnityEngine.UI;
using utils.injection;

public class EscrowNotify : InjectableBehaviour
{
    [Inject]
    private EscrowApi _escrow;
    
    
    [Inject]
    private ResourceModel _resource;
    
    [Inject]
    private SelectedTierModel _tier;

    public GameObject MatchEndPopup;

    public Text MatchResult;
    
    void Start()
    {
        _escrow.Start(_tier.value);
        _escrow.MatchEnd.Add(OnMatchEnd);
    }

    private void OnMatchEnd()
    {
        MatchEndPopup.SetActive(true);
        MatchResult.text = $"yN COLLECTED: {_resource.Get(Faction.Player)}";
    }

    private void OnDestroy()
    {
        _escrow.MatchEnd.Remove(OnMatchEnd);
    }
}