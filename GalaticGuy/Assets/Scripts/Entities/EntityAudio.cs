using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class contains methods for creating audio on individual entities within the scene
/// Open this file to view templates of recommended implentations
/// </summary>
public abstract class EntityAudio : MonoBehaviour
{
    /* example implementation
        [FMODUnity.EventRef]
        private string OneShotEventSound;

        
    */

    /// <summary>
    /// Plays the default sound
    /// </summary>
    public abstract void PlayOneShot();

}
