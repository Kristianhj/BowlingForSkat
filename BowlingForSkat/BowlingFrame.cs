using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingForSkat
{
    internal class NewBowlingFrame
    {
        private List<int> framepoints;
        private bool isBonusFrame;

        private int bonusScore;
        private int accumulatedScore;

        public NewBowlingFrame(List<int> framepoints)
        {
            this.framepoints = framepoints;
        }

        internal int GetPinsDown()
        {
            return framepoints.Sum();
        }

        internal bool IsStrike()
        {
            return framepoints[0] == 10;
        }

        internal bool IsSpare()
        {
            return ((!IsStrike()) && ((framepoints[0] + framepoints[1]) == 10));
        }

        internal bool IsBonusFrame()
        {
            return isBonusFrame;
        }

        internal void SetBonusFrame(bool value)
        {
            isBonusFrame = value;
        }       

        internal int GetThrowScores(ref int remainingBonusThrows)
        {
            //The accumulated bonus
            int addedBonus = 0;

            for (int i = 0; i < framepoints.Count; i++)
            {
                if (!isValidThrow(i))
                    return addedBonus;

                addedBonus += framepoints[i];
                //Subtract from the required bonus throws
                remainingBonusThrows--;

                if (remainingBonusThrows == 0)
                    return addedBonus;
            }

            return addedBonus;
        }

        internal void ApplyToAccumulatedScore(int accumulatedScoreSoFar)
        {
            accumulatedScore = accumulatedScoreSoFar + GetFrameScore();
        }

        internal int GetAccumulatedScore()
        {
            return accumulatedScore;
        }

        internal object GetPinsDownAtThrow(int throwIndex)
        {
            return framepoints[throwIndex];
        }

        internal int GetFrameScore()
        {
            return bonusScore + framepoints.Sum();
        }

        internal void ApplyBonus(int bonusValue)
        {
            bonusScore = bonusValue;
        }

        //Check if the index is actually a throw (or just an automatic zero after a strike)
        private bool isValidThrow(int throwIndex)
        {
            if ((IsStrike() && !isBonusFrame) && throwIndex > 0)
                return false;
            else
                return true;
        }
    }
}