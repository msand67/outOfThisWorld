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
        public Check(int iType = 1, int iDifficulty=10, int iPenalty=2, bool iIsHidden = false, double iTimeToExecute = 10.0, bool iIsRequired = false)
        {
            type = (CheckType)iType;
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

        public (bool, int) PerformCheck(Dictionary<StatType, int> statList, int difficultyMod = 0)
        {
            int attemptValue = UnityEngine.Random.Range(0, 20) + statList[(StatType)type];
            if (attemptValue >= (int)difficulty + difficultyMod)
            {
                return (true, 0);
            }
            return (false, (int)penalty);
        }

    }
}
