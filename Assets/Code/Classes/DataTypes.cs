using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{

    //DC of a check
    public enum checkDifficulty
    {
        pitiful=7,
        easy=10,
        approachable=13,
        complex=16,
        demanding = 19
    }


    //securityPenalty is a modifier to the DC of checks made for each consecutive security level.
    //Currently cut in half.
    public enum securityPenalty 
    {
        low = 1,
        medium = 2,
        high = 3
    }

    //How many seconds are added for a failure after grace period.
    public enum failPenalty 
    {
        low= 1,
        medium=2,
        high=3

    }

    //Security interval is how many seconds pass until security level rises.
    public enum securityInterval //definitely needs to be looked at when balancing
    {
        low = 5,
        medium = 10,
        high = 15
    }
}
