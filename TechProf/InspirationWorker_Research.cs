using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechProf
{
    public class InspirationWorker_Research : InspirationWorker
    {
        // Keep track of how many research points are used up. Once we reach 25000, end the inspiration if it hasn't expired already.

        public void UseInspirationResearchPoints(float pointsLeft)
        {
            if (ResearchPointsLeft > 0)
            {
                ResearchPointsLeft -= pointsLeft;
            }
            else
            {
                throw new Exception("No research points left in Inspiration Worker");
            }
        }

        public float ResearchPointsLeft { get; private set; } = 125000;
    }
}
