namespace BowlingForSkat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class NewBowlingMatch
    {
        public string Token;
        public List<NewBowlingFrame> Frames;

        public NewBowlingMatch(BowlingServerGETData data)
        {
            this.Token = data.token;
            this.Frames = new List<NewBowlingFrame>();

            foreach (List<int> framepoints in data.points)
            {
                Frames.Add(new NewBowlingFrame(framepoints));
            }

            //Mark last frame as bonusFrame
            if (Frames.Count == 11)
                Frames[10].SetBonusFrame(true);

            calculateFrameScores();
        }

        private void calculateFrameScores()
        {
            for (int i = 0; i < Frames.Count; i++)
            {
                //Only do this for the first 10 "Real" frames
                if(!Frames[i].IsBonusFrame())
                {
                    //Apply bonus from subsequent throws
                    if (Frames[i].IsStrike())
                        Frames[i].ApplyBonus(GetValueOfNextThrows(i, 2));
                    else if (Frames[i].IsSpare())
                        Frames[i].ApplyBonus(GetValueOfNextThrows(i, 1));
                }

                int accumulatedScore = 0;
                //If this is not frame 0 
                if (i > 0)
                    accumulatedScore = Frames[i-1].GetAccumulatedScore();

                Frames[i].ApplyToAccumulatedScore(accumulatedScore);
            }
        }

        internal int GetFinalScore()
        {
            //Return the accumulated score of last frame that is not bonusframe
            return Frames.Last<NewBowlingFrame>(val => !val.IsBonusFrame()).GetAccumulatedScore();
        }

        private int GetValueOfNextThrows(int frameIndex, int requiredBonusThrows)
        {
            //The number of throws that still needs to be applied
            int remainingBonusThrows = requiredBonusThrows;

            //The accumulated bonus
            int bonusPoints = 0;

            //No more throws to apply bonus from
            if (Frames.Count <= (frameIndex + 1))
                return 0;
            else
            {
                //The out param shows how many throws still needs to be added
                bonusPoints = Frames[frameIndex + 1].GetThrowScores(ref remainingBonusThrows);
                //Recursively go through frames until we find enough bonus balls
                if (remainingBonusThrows > 0)
                    bonusPoints += GetValueOfNextThrows(frameIndex + 1, remainingBonusThrows);
            }

            return bonusPoints;
        }
    }
}