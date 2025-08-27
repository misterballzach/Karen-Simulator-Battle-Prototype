using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A struct that links an expression name (e.g., "happy", "angry") to a specific sprite.
/// </summary>
[System.Serializable]
public struct CharacterExpression
{
    public string name;
    public Sprite sprite;
}

/// <summary>
/// A ScriptableObject that defines a character in the visual novel.
/// It holds the character's name and a list of their expression sprites.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "KAREN/Dialogue/Character")]
public class Character : ScriptableObject
{
    [Tooltip("The name of the character that will be displayed in the dialogue UI.")]
    public string characterName;

    [Tooltip("A list of expression names and their corresponding sprites.")]
    public List<CharacterExpression> expressions;

    /// <summary>
    /// Gets a character's expression sprite by name.
    /// </summary>
    /// <param name="expressionName">The name of the expression to find (case-insensitive).</param>
    /// <returns>The corresponding sprite, or the first sprite in the list if the name is empty, or null if not found.</returns>
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
