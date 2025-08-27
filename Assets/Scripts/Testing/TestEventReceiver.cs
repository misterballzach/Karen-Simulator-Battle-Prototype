using UnityEngine;

public class TestEventReceiver : MonoBehaviour
{
    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
        Debug.Log("Event triggered: Changed color of the cube!");
    }
}
