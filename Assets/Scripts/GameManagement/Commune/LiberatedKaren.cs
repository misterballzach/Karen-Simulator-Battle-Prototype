[System.Serializable]
public class LiberatedKaren
{
    public string originalName;
    public KarenClass originalClass;
    // Could also include stats, skills, etc. in the future

    public LiberatedKaren(string name, KarenClass karenClass)
    {
        originalName = name;
        originalClass = karenClass;
    }
}
