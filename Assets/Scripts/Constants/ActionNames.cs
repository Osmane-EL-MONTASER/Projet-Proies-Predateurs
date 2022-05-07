/// <summary>
/// Classe contenant toutes les constantes correspondantes
/// au nom des actions du projet.
/// 
/// Fait par EL MONTASER Osmane le 17/04/2022.
/// </summary>
public static class ActionNames {
    public const string IDLE_ACTION = "Idle";
    public const string FIND_FOOD_ACTION = "Find Food";
    public const string EAT_ACTION = "Eat Food";
    public const string CHOOSE_PREY_ACTION = "Choose Prey";
    public const string SLEEP_ACTION = "Sleep";
    public const string BREED_ACTION = "Breed";
    public const string DRINK_ACTION = "Drink Water";

    public const double ENERGY_FACTOR = 2.0 / 86400;
    public const double WATER_FACTOR = 1.5 / 86400;
    public const double STAMINA_FACTOR = 1.0 / 86400;

    public static float TimeSpeed = 86400f;

    public const float DAY_DURATION = 86400f;
}
