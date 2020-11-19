using System.Collections.Generic;

namespace OrbitalWitness.Models
{
    public class LeassessScheduleEntry
    {
        public string DateAndPlanRef { get; set; }
        public string PropertyDescription { get; set; }
        public string DateOfLeaseAndTerm { get; set; }
        public string LesseeTitle { get; set; }
        public IEnumerable<string> Notes { get; set; }
    }
}
