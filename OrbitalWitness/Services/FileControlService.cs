using Newtonsoft.Json;
using OrbitalWitness.Models.DTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrbitalWitness.Services
{
    public class FileControlService
    {
        public IEnumerable<LeassesScheduleDTO> ReadLeasesFromFile(string filePath)
        {
            return JsonConvert.DeserializeObject<IEnumerable<RootDTO>>(File.ReadAllText(filePath)).Select(rootObject => rootObject.Leaseschedule).ToList();
        }
    }
}
