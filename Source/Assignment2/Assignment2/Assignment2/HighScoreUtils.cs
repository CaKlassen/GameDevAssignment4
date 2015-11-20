using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;

namespace Assignment2
{
    public static class HighScoreUtils
    {
        public static int NUM_STORED_SCORES = 10;

        private static int[] TEMPLATE_SCORES = { 10000, 9000, 8000, 7000, 5000, 4000, 3000, 2000, 1000, 500 };

        /// <summary>
        /// This function generates a template high score list for first-time players.
        /// </summary>
        /// <returns>A template high score list</returns>
        public static List<int> createInitialHighScores()
        {
            List<int> templateHighScore = new List<int>();

            for (int i = 0; i < TEMPLATE_SCORES.Length; i++)
            {
                templateHighScore.Add(TEMPLATE_SCORES[i]);
            }

            return templateHighScore;
        }

        /// <summary>
        /// This function updates the high score list with a new score. If the new score is high enough
        /// to place in the list, it will be inserted, otherwise it will not.
        /// </summary>
        /// <param name="highScoreList">The current high score list</param>
        /// <param name="newScore">The new score to attempt to insert</param>
        public static void updateHighScores(List<int> highScoreList, int newScore)
        {
            for (int i = 0; i < NUM_STORED_SCORES; i++)
            {
                // If the new score is greater than this score
                if (newScore > highScoreList[i])
                {
                    // Insert the new score
                    highScoreList.Insert(i, newScore);

                    // Remove the bottom score
                    highScoreList.RemoveAt(10);

                    break;
                }
            }
        }

    }
}
