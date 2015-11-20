using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    
    public class AudioUtils
    {
        private static AudioUtils instance;
        public Song day;
        public Song night;
        public float curVol;

        /// <summary>
        /// The constructor for the audio utils class.
        /// </summary>
        public AudioUtils()
        {
            // This constructor should be used for setting default values, etc.


        }

        /// <summary>
        /// This function loads the music and sound effects.
        /// </summary>
        /// <param name="c">The content manager for loading audio.</param>
        public void loadContent(ContentManager c)
        {
            // Load sound effects
            //sndBounce = c.Load<SoundEffect>("SFX/sndBounce");

            // Load music
            day = c.Load<Song>("Sound/A Ruined Village");
            night = c.Load<Song>("Sound/Cold Sweat");
            MediaPlayer.Volume = 1.0f;
        }

        /// <summary>
        /// fades the current track out
        /// </summary>
        /// <returns>true when track is faded to 0; false otherwise</returns>
        public bool fadeTrackOut()
        {
            bool fadeComplete = false;
            MediaPlayer.Volume += PhysicsUtil.smoothChange(MediaPlayer.Volume, 0, 40);
            float volume = MediaPlayer.Volume;
            if (MediaPlayer.Volume < 0.1)
            {
                MediaPlayer.Volume = 0.0f;
                fadeComplete = true;
            }
                

            return fadeComplete;
        }

        /// <summary>
        /// fades the current track in
        /// </summary>
        /// <returns>true when track is faded in to initial volume before fadeOut; false otherwise</returns>
        public bool fadeTrackIn()
        {
            bool fadedComplete = false;
            MediaPlayer.Volume += PhysicsUtil.smoothChange(MediaPlayer.Volume, curVol, 5);

            if(MediaPlayer.Volume > curVol - 0.001)
            {
                MediaPlayer.Volume = curVol;
                fadedComplete = true;
            }

            return fadedComplete;
        }

        /// <summary>
        /// This function returns an instance of the audio utils object.
        /// </summary>
        /// <returns>An instance of the audio utils object</returns>
        public static AudioUtils getInstance()
        {
            if (instance == null)
            {
                instance = new AudioUtils();
            }

            return instance;
        }
    }
}
