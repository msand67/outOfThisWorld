using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace dataStructures
{
    [Serializable]
    public class Check
    {
        public CheckType type;
        public CheckDifficulty difficulty;
        public FailPenalty penalty;
        public bool isHidden;
        public double timeToExecute;
        public bool isRequired;
        public bool isComplete;
        public Check(int iType = 1, int iDifficulty=10, int iPenalty=2, bool iIsHidden = false, double iTimeToExecute = 10.0, bool iIsRequired = false, bool iIsComplete=false)
        {
            type = (CheckType)iType;
            difficulty = (CheckDifficulty)iDifficulty;
            penalty = (FailPenalty)iPenalty;
            isHidden = iIsHidden;
            timeToExecute = iTimeToExecute;
            isRequired = iIsRequired;
            isComplete = iIsComplete;
        }
        public Check(CheckType iType = CheckType.Breach, CheckDifficulty iDifficulty=CheckDifficulty.easy, FailPenalty iPenalty = FailPenalty.medium, bool iIsHidden = false, double iTimeToExecute = 10.0, bool iIsRequired =false, bool iIsComplete=false)
        {
            type = iType;
            difficulty = iDifficulty;
            penalty = iPenalty;
            isHidden = iIsHidden;
            timeToExecute = iTimeToExecute;
            isRequired = iIsRequired;
            isComplete = iIsComplete;
        }

        public (bool, int, double) PerformCheck(List<Stat> statList, int difficultyMod = 0, int failureBonus = 0)
        {
            int attemptValue = UnityEngine.Random.Range(0, 20);
            int attemptBonus = statList[(int)type].level + failureBonus;
            int ratio = ((int)this.difficulty + difficultyMod) - (statList[(int)type].level + failureBonus);
            if (attemptValue + attemptBonus >= (int)difficulty + difficultyMod || attemptValue==20)
            {
                isComplete = true;
                Debug.Log($"Check succeeded with {(double)(20-ratio) / 20} chance of success.");
                return (true, 0, 0);
            }
            Debug.Log($"Check failed with {(double)(20-ratio) / 20} chance of success.");
            return (false, (int)penalty, timeToExecute);
        }

    }
}
