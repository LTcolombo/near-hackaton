using api;
using service;
using UnityEngine;
using utils.injection;
using view.behaviours.UI.nft;

public class SelectHandler : InjectableBehaviour
{
    [Inject] private NFTSelectionService _selectionService;
    
    public GameObject selectedObj;
    public GameObject deselectedObj;

    private bool _selected;

    public void SetSelected(bool value)
    {
        _selected = value;
        selectedObj.SetActive(value);
        deselectedObj.SetActive(!value);
    }

    public void OnClick()
    {
        var data = GetComponent<DisplayNFT>()?.Data ?? default(NFT);

        if (data.Id == null)
            return;

        if (_selectionService.TrySetState(data.Id, !_selected))
        {
            SetSelected(!_selected);
        }
    }
}
