using UnityEngine;
using System.Collections.Generic;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile s_instance;

    public List<Card> masterCardCollection = new List<Card>();
    public List<Card> currentDeck = new List<Card>();

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
