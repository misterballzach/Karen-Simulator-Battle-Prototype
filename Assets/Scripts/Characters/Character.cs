using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct CharacterExpression
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "New Character", menuName = "KAREN/Dialogue/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public List<CharacterExpression> expressions;

    public Sprite GetSprite(string expressionName)
    {
        if (string.IsNullOrEmpty(expressionName))
        {
            return expressions.Count > 0 ? expressions[0].sprite : null;
        }

        var expression = expressions.FirstOrDefault(e => e.name.Equals(expressionName, System.StringComparison.OrdinalIgnoreCase));
        return expression.sprite; // Returns null if not found, which is intended
    }
}
