using System.Runtime.InteropServices;
using UnityEngine.UI;
using utils.injection;

namespace view.behaviours.UI.nft
{
    public class ConfirmDeposit : InjectableBehaviour
    {
        public Text text;
        
        [DllImport("__Internal")]
        private static extern void Deposit(int value);
        
        public void DepositConfirmed()
        {
            Deposit(int.Parse(text.text));
            
            text.text = "";
        }
        
    }
}