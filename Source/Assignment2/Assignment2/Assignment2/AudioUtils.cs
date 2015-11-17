using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment2
{
    public class AudioUtils
    {
        private static AudioUtils instance;

        // Sound effects
        public SoundEffect sndBounce;

        // Music
        public Song mscGame;

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
            sndBounce = c.Load<SoundEffect>("SFX/sndBounce");

            // Load music
            mscGame = c.Load<Song>("SFX/mscGame");
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
