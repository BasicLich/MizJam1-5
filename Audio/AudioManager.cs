using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Audio
{

    /// <summary>
    /// The Audio Manager takes care of playing sounds and music and the audio listener. 
    /// Both of these are used for positioning the sound effects in a 3D space.
    /// </summary>
    public class AudioManager
    {
        public static AudioManager Instance = new AudioManager();

        private Dictionary<string, Song> songs;
        private Dictionary<string, SoundEffect> soundFXs;

        private AudioListener audioListener;

        public AudioManager()
        {
            songs = new Dictionary<string, Song>();
            soundFXs = new Dictionary<string, SoundEffect>();

            SoundEffect.DistanceScale = 256f;

            audioListener = new AudioListener
            {
                Position = Vector3.Zero
            };

            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Adds the given song to the audio manager.
        /// </summary>
        /// <param name="songPath"></param>
        /// <param name="song"></param>
        public void AddSong(string songPath, Song song)
        {
            songs[songPath] = song;
        }

        /// <summary>
        /// Adds the given sound effect to the audio manager.
        /// </summary>
        /// <param name="soundEffectPath"></param>
        /// <param name="soundEffect"></param>
        public void AddSoundEffect(string soundEffectPath, SoundEffect soundEffect)
        {
            soundFXs[soundEffectPath] = soundEffect;
        }

        /// <summary>
        /// Sets the volume of the music.
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// Sets the volume of sound effects
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundEffectVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }

        /// <summary>
        /// Updates the position of the listener.
        /// </summary>
        /// <param name="position"></param>
        public void UpdateListenerPosition(Vector2 position)
        {
            audioListener.Position = new Vector3(position, 300);
        }

        /// <summary>
        /// Plays the given song.
        /// </summary>
        /// <param name="song"></param>
        public void PlaySong(string song)
        {
            MediaPlayer.Play(songs[song]);
        }

        /// <summary>
        /// Stops the playing song.
        /// </summary>
        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Plays the given sound at the position given, with a throw-away emitter.
        /// </summary>
        /// <param name="soundEffect"></param>
        /// <param name="position"></param>
        public void PlaySoundEffect(string soundEffect, Vector2 position)
        {
            AudioEmitter audioEmitter = new AudioEmitter
            {
                Position = new Vector3(position, 0)
            };

            SoundEffectInstance sndFX = soundFXs[soundEffect].CreateInstance();

            sndFX.Apply3D(audioListener, audioEmitter);

            sndFX.Play();
        }

    }
}
