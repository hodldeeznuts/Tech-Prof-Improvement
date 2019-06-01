using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TechProf
{
    public class CompUseEffect_ResearchInspiration : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            this.CauseResearchInspiration(usedBy);
        }

        // Check if the pawn selected can get an intellectual inspiration

        public override bool CanBeUsedBy(Pawn p, out string failReason)
        {
            // If pawn is incapable of intellectual do not allow the inspiration
            if (p.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
            {
                failReason = p.Name.ToStringShort.CapitalizeFirst() + " is not capable of " + SkillDefOf.Intellectual.LabelCap;
                return false;
            }
            // If pawn is already inspired do not allow the inspiration
            else if (p.mindState.inspirationHandler.Inspired)
            {
                failReason = p.Name.ToStringShort.CapitalizeFirst() + " is already inspired";
                return false;
            }

            // Pawn can accept inspiration
            failReason = null;
            return true;
        }

        private void CauseResearchInspiration(Pawn usedBy)
        {
            // Try to give pawn inspiration, if it fails throw an exception
            if (!usedBy.mindState.inspirationHandler.TryStartInspiration(DefDatabase<InspirationDef>.GetNamed("Frenzy_Research")))
            {
                throw new Exception("Pawn was unable to accept inspiration");
            }
        }
    }
}
