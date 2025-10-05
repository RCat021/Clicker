using UnityEngine;

public class Click : MonoBehaviour
{
    
   

    public void AddMoney()
    {
               Debug.Log("Clicked");
        CoinManager.AddCoins(1);
    }
}
