using System;
using System.Collections.Generic;

namespace BowlingForSkat
{
    internal class BowlingServerPOSTData
    {
        public BowlingServerPOSTData(NewBowlingMatch match)
        {
            this.token = match.Token;
            points = new List<int>();

            foreach (NewBowlingFrame frame in match.Frames)
            {
                if(!frame.IsBonusFrame())
                    points.Add(frame.GetAccumulatedScore());
            }
        }

        public List<int> points { get; set; }
        public string token { get; set; }
    } 
}