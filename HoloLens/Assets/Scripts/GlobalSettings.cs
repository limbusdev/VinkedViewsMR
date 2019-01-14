public class GlobalSettings
{
    // Only RELEASE enables persistence
    public enum Scenario
    {
       DEBUG_MetaVis, DEBUG_VisBridges, RELEASE
    }

    public static bool onHoloLens = false;
    public static bool DEBUG = false;
    public static Scenario scenario = Scenario.RELEASE;
}
