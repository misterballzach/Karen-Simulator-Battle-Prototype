using UnityEngine;

public class MindscapeManager : MonoBehaviour
{
    public static MindscapeManager s_instance;

    public Mindscape currentMindscape;
    private int currentEventIndex = -1;

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

    public void StartMindscape(Mindscape mindscape)
    {
        currentMindscape = mindscape;
        currentEventIndex = -1;
        Debug.Log($"Entering mindscape: {mindscape.mindscapeName}");
        AdvanceToNextEvent();
    }

    public void AdvanceToNextEvent()
    {
        currentEventIndex++;
        if (currentMindscape != null && currentEventIndex < currentMindscape.events.Count)
        {
            MindscapeEvent nextEvent = currentMindscape.events[currentEventIndex];
            nextEvent.Trigger();
        }
        else
        {
            Debug.Log($"Mindscape {currentMindscape.mindscapeName} completed!");
            // Return to a hub world or commune scene
        }
    }
}
