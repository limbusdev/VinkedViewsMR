public class GlobalSettings
{
    public enum Scenario
    {
        TEST_BostonPD, TEST_FBI, TEST_Exoplanets, TEST_MetaVis, TEST_Playground, TEST_empty
    }

    public static bool onHoloLens = false;
    public static bool DEBUG = false;
    public static Scenario scenario = Scenario.TEST_empty;
}
