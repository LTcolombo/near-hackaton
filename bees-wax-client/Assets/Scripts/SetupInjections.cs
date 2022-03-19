using utils.injection;
using UnityEngine;

public class SetupInjections : MonoBehaviour
{
    private void Awake()
    {
        Injector.Setup(GetType().Assembly);
    }
}