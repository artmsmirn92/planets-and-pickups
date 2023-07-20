using System;
using System.Runtime.CompilerServices;
using AudioYB;
using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A primitive sound manager, playing <see cref="AudioSource"/>s when called.
    ///
    /// For a bigger project I'd recommend MasterAudio from the Unity Asset Store.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private AudioYb death;
        [SerializeField] private AudioYb pickup;
        [SerializeField] private AudioYb jump;
        [SerializeField] private AudioYb touchPlanet;
        [SerializeField] private AudioYb enemySpawned;
        [SerializeField] private AudioYb mainTheme;

        #endregion

        #region api

        public void PlaySound(Sound _Sound)
        {
            string audioClipName = GetAudioClipName(_Sound);
            var audioSource = GetAudioSource(_Sound);
            if (audioSource != null)
                audioSource.Play(audioClipName);
        }

        public void MuteSound(bool _Mute)
        {
            mainTheme.Volume = _Mute ? 0f : 1f;
        }

        public void EnableSound(bool _IsOn)
        {
            death.Enabled        = _IsOn;
            pickup.Enabled       = _IsOn;
            jump.Enabled         = _IsOn;
            touchPlanet.Enabled  = _IsOn;
            enemySpawned.Enabled = _IsOn;
            mainTheme.Enabled    = _IsOn;
        }

        #endregion

        #region nonpublic methods

        private static string GetAudioClipName(Sound _Sound)
        {
            return _Sound switch
            {
                Sound.Death        => "death",
                Sound.Pickup       => "pickup",
                Sound.Jump         => "jump",
                Sound.TouchPlanet  => "touch_planet",
                Sound.EnemySpawned => "enemy_spawn",
                Sound.MainTheme    => "main_theme",
                _                  => throw new SwitchExpressionException(_Sound)
            };
        }

        private AudioYb GetAudioSource(Sound _Sound)
        {
            return _Sound switch
            {
                Sound.Death        => death,
                Sound.Pickup       => pickup,
                Sound.Jump         => jump,
                Sound.TouchPlanet  => touchPlanet,
                Sound.EnemySpawned => enemySpawned,
                Sound.MainTheme    => mainTheme,
                _                  => throw new NotImplementedException(_Sound.ToString())
            };
        }

        #endregion
    }
}