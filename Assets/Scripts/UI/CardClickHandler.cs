using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Card cardData;
    private BattleSystem battleSystem;

    void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleSystem != null && battleSystem.state == BattleState.PlayerTurn)
        {
            // In a real game, a targeting system would be needed.
            // For now, assume the first enemy is the target.
            Entity target = battleSystem.enemy;
            battleSystem.OnCardPlayed(cardData, target);
        }
    }
}
