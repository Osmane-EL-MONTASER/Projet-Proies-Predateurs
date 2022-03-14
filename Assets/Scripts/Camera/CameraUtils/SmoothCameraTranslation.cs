using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cette classe permet de réaliser une translation de la caméra
/// de façon esthétique.
/// 
/// Fait par EL MONTASER Osmane le 12/03/2022.
/// </summary>
public class SmoothCameraTranslation {
    /// <summary>
    /// La vitesse de la translation de la caméra qu'elle parcourt
    /// en un seul tick.
    /// </summary>
    private float _translationSpeed;

    /// <summary>
    /// Pour contrôler la Translation pour la transition entre
    /// les 2 caméras.
    /// </summary>
    private float _currentTranslation;

    /// <summary>
    /// La translation de la caméra au départ.
    /// </summary>
    private float _startTranslation;

    /// <summary>
    /// La translation de la caméra cible.
    /// </summary>
    private float _targetTranslation;
    
    /// <summary>
    /// Permet d'instancier l'objet SmoothCameraTranslation et
    /// de rénitialiser sa translation courante.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    /// <param name="startTranslation">
    /// La translation de la caméra au départ.
    /// </param>
    /// <param name="targetTranslation">
    /// La translation de la caméra à avoir à la fin de la
    /// translation.
    /// </param>
    /// <param name="translationSpeed">
    /// Le ratio de la vitesse de translation de la caméra
    /// en fonction de la translation à parcourir. 
    /// </param>
    public SmoothCameraTranslation(float startTranslation, float targetTranslation, float translationSpeed = 800f) {
        _startTranslation = startTranslation;
        _targetTranslation = targetTranslation;
        _currentTranslation = startTranslation;
        _translationSpeed = translationSpeed;
    }

    /// <summary>
    /// Permet de récupérer la prochaine valeur de translation
    /// de la caméra à chaque tick.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    /// <returns>
    /// La prochaine valeur de translation si il y en a une.
    /// Retourne -1 s'il n'y en a plus.
    /// </returns>
    public float GetNextTranslation() {
        float translationDirection;

        if(_startTranslation <= _targetTranslation)
            translationDirection = 1f;
        else
            translationDirection = -1f;
        
        if(translationDirection == 1f && _currentTranslation >= _targetTranslation)
            return -1f;
        else if(translationDirection == -1f && _currentTranslation <= _targetTranslation)
            return -1f;

        _currentTranslation += translationDirection * _translationSpeed * Time.deltaTime;

        if(translationDirection == 1f && _currentTranslation >= _targetTranslation)
            return -1f;
        else if(translationDirection == -1f && _currentTranslation <= _targetTranslation)
            return -1f;

        return _currentTranslation;
    }
}
