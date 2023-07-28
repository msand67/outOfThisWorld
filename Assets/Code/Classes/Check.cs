using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Check
    {
        public checkDifficulty difficulty;
        public failPenalty penalty;
        public Check()
        {
            difficulty = checkDifficulty.easy;
            penalty = failPenalty.medium;
        }
        public Check(int iDifficulty, int iPenalty)
        {
            difficulty = (checkDifficulty)iDifficulty;
            penalty = (failPenalty)iPenalty;
        }
        public Check(checkDifficulty iDifficulty, failPenalty iPenalty)
        {
            difficulty = iDifficulty;
            penalty = iPenalty;
        }

        public (bool, int) PerformCheck(int attemptValue, int difficultyMod = 0)
        {
            if (attemptValue >= (int)difficulty + difficultyMod)
            {
                return (true, 0);
            }
            return (false, (int)penalty);
        }

    }
}
