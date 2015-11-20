using Assignment3.Scenes;
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
        public SoundEffect footstep1;
        public SoundEffect footstep2;
        public SoundEffect footstep3;
        public SoundEffect WallBump;
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
            footstep1 = c.Load<SoundEffect>("Sound/Footstep_Cave1");
            footstep2 = c.Load<SoundEffect>("Sound/Footstep_Cave2");
            footstep3 = c.Load<SoundEffect>("Sound/Footstep_Cave3");
            WallBump = c.Load<SoundEffect>("Sound/WallBump");
            // Load music
            day = c.Load<Song>("Sound/A Ruined Village");
            night = c.Load<Song>("Sound/Cold Sweat");
            MediaPlayer.Volume = 1.0f;
            curVol = MediaPlayer.Volume;
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

        public void playFootstep()
        {
            if(MazeScene.instance.camera.moving)
            {          
                Random r = new Random();

                switch (r.Next(1, 3))
                {
                    case 1:
                        footstep1.Play();
                        break;
                    case 2:
                        footstep2.Play();
                        break;
                    case 3:
                        footstep3.Play();
                        break;
                }
                
            }
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
