using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayer
{
    private static StartPlayer instance = null;

    public static StartPlayer Instance
    {
        get
        {
            if(instance == null)
            {
                return new StartPlayer();
            }

            return instance;
        }
    }

    public int maxPlayer;
}
