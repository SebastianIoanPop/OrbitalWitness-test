using OrbitalWitness.Models;
using OrbitalWitness.Services;
using System;
using System.Collections.Generic;

namespace OrbitalWitness
{
    class Program
    {
        static void Main()
        {
            var fileControlService = new FileControlService();
            var inputLeases = fileControlService.ReadLeasesFromFile("inputSample.json");

            var parserService = new LeassesScheduleParserService();

            var outputEntries = new List<LeassessScheduleEntry>();
            foreach(var lease in inputLeases)
            {
                outputEntries.AddRange(parserService.Parse(lease));
            }

            Console.ReadKey();
        }
    }
}
