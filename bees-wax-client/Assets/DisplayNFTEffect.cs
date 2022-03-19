using System.Collections.Generic;
using api;
using service;
using UnityEngine.UI;
using utils.injection;

public class DisplayNFTEffect : InjectableBehaviour
{
    public Text label;

    [Inject] private NFTApi _nft;
    
    [Inject] private NFTSelectionService _selectionService;

    private void Start()
    {
        Redraw();
        _selectionService.Selected.Add(Redraw);
    }

    private void Redraw()
    {
        var text = new List<string>();

        if (_selectionService.Effect.harvest > 1)
            text.Add($"Harvest: +{_selectionService.Effect.harvest - 1:P0}");

        if (_selectionService.Effect.damage > 1)
            text.Add($"Damage: +{_selectionService.Effect.damage - 1:P0}");

        if (_selectionService.Effect.armour > 1)
            text.Add($"Armour: +{_selectionService.Effect.armour - 1:P0}");

        if (_selectionService.Effect.build > 1)
            text.Add($"Build: +{_selectionService.Effect.build - 1:P0}");

        if (_selectionService.Effect.hp > 1)
            text.Add($"HP: +{_selectionService.Effect.hp - 1:P0}");
        
        if (_selectionService.Effect.hatch > 1)
            text.Add($"Hatch: +{_selectionService.Effect.hatch - 1:P0}");
        
        if (_selectionService.Effect.feed > 1)
            text.Add($"Feed: +{_selectionService.Effect.feed - 1:P0}");

        label.text = string.Join("\n", text);
    }
}