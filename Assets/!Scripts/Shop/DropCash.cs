using UnityEngine;

namespace Capstone
{
    public class DropCash : MonoBehaviour
    {
        public void CashToDrop(int amount)
        {
            Wallet.GiveCash(amount);
        }
    }
}
