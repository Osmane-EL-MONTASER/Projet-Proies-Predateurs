using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe qui contient les fonctions à exécuter lors des
/// interactions avec l'interface météorologique.
/// 
/// Fait par EL MONTASER Osmane le 01/03/2022.
/// </summary>
public class WeatherEvents : MonoBehaviour {

    /// <summary>
    /// Le panel qui s'affiche lors du clique sur le bouton
    /// météo.
    /// </summary>
    public GameObject weatherPanel;

    /// <summary>
    /// Permet au code de désactiver la scrollView lorsque le
    /// panel de météo est caché.
    /// </summary>
    public GameObject weatherScrollView;

    /// <summary>
    /// Référence à la caméra afin de changer la vue basique
    /// en la vue d'édition météorologique.
    /// </summary>
    public GameObject Camera;

    /// <summary>
    /// Permet de changer d'effet météorologique si l'utilisateur
    /// se trouve dans l'éditeur météo.
    /// Sinon la vue change entre vue de base et édition météo.
    /// </summary>
    private static WeatherType _oldSelectedWeather;

    /// <summary>
    /// Exécutée au début afin de cacher par défaut tout le
    /// panel de météo.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    void Start() {
        if(this.name == UINames.OPEN_WEATHER_BUTTON)
            toogleWeatherPanel();
        _oldSelectedWeather = WeatherType.None;
    }

    /// <summary>
    /// Exécutée lors d'un clic sur un des boutons de 
    /// l'interface.
    ///
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    public void onClick() {
        if(this.name == UINames.OPEN_WEATHER_BUTTON)
            toogleWeatherPanel();
        else if(this.name == UINames.WIND_WEATHER_BUTTON)
            toogleWeatherZoning(WeatherType.Wind);
        else if(this.name == UINames.THUNDERSTORM_WEATHER_BUTTON)
            toogleWeatherZoning(WeatherType.Thunderstorm);
        else if(this.name == UINames.STORM_WEATHER_BUTTON)
            toogleWeatherZoning(WeatherType.Storm);
        else if(this.name == UINames.DROUGHT_WEATHER_BUTTON)
            toogleWeatherZoning(WeatherType.Drought);
        else if(this.name == UINames.RAIN_WEATHER_BUTTON)
            toogleWeatherZoning(WeatherType.Rain);
    }

    /// <summary>
    /// Exécutée lorsque l'on clique sur une des météos
    /// disponibles dans l'onglet Météo.
    /// Cela permet d'activer l'outil de zoning (brush) 
    /// pour définir plusieurs zones de tailles
    /// différentes avec leurs propres effets 
    /// météorologiques.
    /// 
    /// Fait par EL MONTASER Osmane le 11/03/2022.
    /// </summary>
    private void toogleWeatherZoning(WeatherType selectedWeather) {
        if(_oldSelectedWeather == WeatherType.None) {
            Camera.GetComponent<WeatherCamera>().EnterWeatherLook(selectedWeather);
            _oldSelectedWeather = selectedWeather;
        } else if(_oldSelectedWeather == selectedWeather) {
            Camera.GetComponent<WeatherCamera>().ExitWeatherLook();
            _oldSelectedWeather = WeatherType.None;
        } else {
            Camera.GetComponent<WeatherCamera>().ChangeSelectedWeather(selectedWeather);
            _oldSelectedWeather = selectedWeather;
        }
    }

    /// <summary>
    /// Fonction permettant d'afficher ou de cacher le panel
    /// météo avec toutes les météos que l'on peut ajouter.
    /// 
    /// Fait par EL MONTASER Osmane le 01/03/2022.
    /// </summary>
    private void toogleWeatherPanel() {
        var weatherPanelGroup = weatherPanel.GetComponent<CanvasGroup>();
        if(weatherPanelGroup.alpha == 0) {
            weatherPanelGroup.alpha = 1;
            weatherPanelGroup.interactable = true;
            weatherScrollView.GetComponent<ScrollRect>().enabled = true;
        } else {
            weatherPanelGroup.alpha = 0;
            weatherPanelGroup.interactable = false;
            weatherScrollView.GetComponent<ScrollRect>().enabled = false;
        }
        
    }
}
