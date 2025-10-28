using UnityEngine;

/// <summary>
/// A simple component that has a public method that can be targeted by a UnityEvent.
/// Used for testing the dialogue system's event triggers.
/// </summary>
public class TestEventReceiver : MonoBehaviour
{
    /// <summary>
    /// Changes the color of the GameObject's material to a random color.
    /// </summary>
    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
        Debug.Log("Event triggered: Changed color of the cube!");
    }
}
