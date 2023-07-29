using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    [Serializable]
    public class Agent
    {
        public List<Stat> statList;
        public string name;
        public int id;
        public int currentRoom;
        public bool isInside;

        public Agent()
        {
            statList = new List<Stat>();
            statList.Add(new Stat(StatType.Breach, 0));
            statList.Add(new Stat(StatType.Egghead, 0));
            statList.Add(new Stat(StatType.Smoothtalk, 0));
            statList.Add(new Stat(StatType.Gumshoe, 0));
            statList.Add(new Stat(StatType.Footwork, 0));
            statList.Add(new Stat(StatType.FastHands, 0));

            name = "Murphy";
            id = -1;
            currentRoom = -1;
            isInside = false;
        }
        public Agent(List<Stat> iStatList, string iName, int iId, int iCurrentRoom, bool iIsInside)
        {
            statList = iStatList;
            name = iName;
            id = iId;
            currentRoom = iCurrentRoom;
            isInside = iIsInside;
        }
        public int GetCheckMod(CheckType type)
        {
            return (int)statList[(int)type].type;
        }

        internal double ComputeTime(double time, AgentAction action)
        {
            switch(action){
                case AgentAction.MakeCheck:
                    return time * (1+(statList[(int)StatType.FastHands].level * 0.05));
                case AgentAction.Move:
                    return time * (1+(statList[(int)StatType.Footwork].level * 0.05));
                default:
                    return time;
            }
        }

        internal string GetBestStat()
        {
            Stat highestStat = new Stat(StatType.Breach, -1);
            string statlist = "";

            foreach (Stat stat in statList)
            {
                if (stat.level > highestStat.level)
                {
                    highestStat = stat;
                    statlist = stat.type.ToString();
                }else if (stat.level == highestStat.level)
                {
                    statlist += $", {stat.type.ToString()}";
                }

            }
            return statlist;
        }
    }

    [Serializable]
    public class Stat
    {
        public StatType type;
        public int level;

        public Stat(StatType iType, int iLevel)
        {
            type = iType;
            level = iLevel;
        }
    }
}