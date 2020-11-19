using System.Collections.Generic;

namespace OrbitalWitness.Models.DTO
{
    public class LeassesScheduleDTO
    {
        public string ScheduleType { get; set; }
        public IEnumerable<ScheduleEntryDTO> ScheduleEntry { get; set; }
    }
}
