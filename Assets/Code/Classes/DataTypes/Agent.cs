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
        public int cost;
        public string roleDesc;
        public Sprite sprite;

        public Agent()
        {
            statList = new List<Stat>();
            statList.Add(new Stat(StatType.Breach, 0));
            statList.Add(new Stat(StatType.Egghead, 0));
            statList.Add(new Stat(StatType.Smoothtalk, 0));
            statList.Add(new Stat(StatType.Gumshoe, 0));
            statList.Add(new Stat(StatType.Footwork, 0));
            statList.Add(new Stat(StatType.FastHands, 0));
            cost = 15000;
            roleDesc = "Look, he's cheap and sometimes does stuff. What more do you want?/n--The Recruiter";

            name = "Murphy";
            id = -1;
            currentRoom = -1;
            isInside = false;
        }
        public Agent(List<Stat> iStatList, string iName, int iId, int iCurrentRoom, bool iIsInside, int iCost, string iRoleDesc)
        {
            statList = iStatList;
            name = iName;
            id = iId;
            currentRoom = iCurrentRoom;
            isInside = iIsInside;
            cost = iCost;
            roleDesc = iRoleDesc;
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

        internal string GetStatNums()
        {
            string statNums = "";
            foreach(Stat s in statList)
            {
                statNums += $"{s.level}\n";
            }
            return statNums;
        }

        internal string GetStatText()
        {

            string statNums = "";
            foreach (Stat s in statList)
            {
                statNums += $"{s.type.ToString()}\n";
            }
            return statNums;
        }
        internal string GetStatAbrv()
        {
            return "B\nE\nST\nG\nF\nFH";
        }

        internal string GetDescriptionText()
        {
            string descText = "";
            descText += $"Name: {name}";
            descText += $"\nCost: {cost}";

            return descText;
        }
        internal int GetCost()
        {
            return cost;
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