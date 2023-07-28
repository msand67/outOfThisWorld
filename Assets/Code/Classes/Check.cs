using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Check
    {
        public CheckType type;
        public CheckDifficulty difficulty;
        public FailPenalty penalty;
        public bool isHidden;
        public double timeToExecute;
        public bool isRequired;
        public Check(CheckType iType = CheckType.Breach, int iDifficulty=10, int iPenalty=2, bool iIsHidden = false, double iTimeToExecute = 10.0, bool iIsRequired = false)
        {
            type = iType;
            difficulty = (CheckDifficulty)iDifficulty;
            penalty = (FailPenalty)iPenalty;
            isHidden = iIsHidden;
            timeToExecute = iTimeToExecute;
            isRequired = iIsRequired;
        }
        public Check(CheckType iType = CheckType.Breach, CheckDifficulty iDifficulty=CheckDifficulty.easy, FailPenalty iPenalty = FailPenalty.medium, bool iIsHidden = false, double iTimeToExecute = 10.0, bool iIsRequired =false)
        {
            type = iType;
            difficulty = iDifficulty;
            penalty = iPenalty;
            isHidden = iIsHidden;
            timeToExecute = iTimeToExecute;
            isRequired = iIsRequired;
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
