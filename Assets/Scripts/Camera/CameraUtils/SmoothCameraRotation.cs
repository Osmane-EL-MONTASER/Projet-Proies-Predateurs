using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cette classe permet de réaliser une rotation de la caméra
/// de façon esthétique.
/// 
/// Fait par EL MONTASER Osmane le 12/03/2022.
/// </summary>
public class SmoothCameraRotation {
    /// <summary>
    /// La vitesse de la rotation de la caméra qu'elle parcourt
    /// en un seul tick.
    /// </summary>
    private float _rotationSpeed;

    /// <summary>
    /// Pour contrôler la rotation pour la transition entre
    /// les 2 caméras.
    /// </summary>
    private float _currentRotation;

    /// <summary>
    /// La rotation de la caméra au départ.
    /// </summary>
    private float _startRotation;

    /// <summary>
    /// La rotation de la caméra cible.
    /// </summary>
    private float _targetRotation;
    
    /// <summary>
    /// Permet d'instancier l'objet SmoothCameraRotation et
    /// de rénitialiser sa rotation courante.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    /// <param name="startRotation">
    /// La rotation de la caméra au départ.
    /// </param>
    /// <param name="targetRotation">
    /// La rotation de la caméra à avoir à la fin de la
    /// rotation.
    /// </param>
    /// <param name="rotationSpeed">
    /// Le ratio de la vitesse de rotation de la caméra
    /// en fonction de la rotation à parcourir. 
    /// </param>
    public SmoothCameraRotation(float startRotation, float targetRotation, float rotationSpeed = 100f) {
        _startRotation = startRotation;
        _targetRotation = targetRotation;
        _currentRotation = startRotation;
        _rotationSpeed = rotationSpeed;
    }

    /// <summary>
    /// Permet de récupérer la prochaine valeur de rotation
    /// de la caméra à chaque tick.
    /// 
    /// Fait par EL MONTASER Osmane le 12/03/2022.
    /// </summary>
    /// <returns>
    /// La prochaine valeur de rotation si il y en a une.
    /// Retourne -1 s'il n'y en a plus.
    /// </returns>
    public float GetNextRotation() {
        float rotationDirection;
        if(_startRotation <= _targetRotation)
            rotationDirection = 1f;
        else
            rotationDirection = -1f;

        if(rotationDirection == 1f && _currentRotation >= _targetRotation)
            return -1f;
        else if(rotationDirection == -1f && _currentRotation <= _targetRotation)
            return -1f;

        _currentRotation += rotationDirection * _rotationSpeed * Time.deltaTime;

        if(rotationDirection == 1f && _currentRotation >= _targetRotation)
            return _targetRotation;
        else if(rotationDirection == -1f && _currentRotation <= _targetRotation)
            return _targetRotation;

        return _currentRotation;
    }
}
