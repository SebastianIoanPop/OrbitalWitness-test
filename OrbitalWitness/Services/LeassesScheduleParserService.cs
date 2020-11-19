using OrbitalWitness.Models;
using OrbitalWitness.Models.DTO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OrbitalWitness.Services
{
    public class LeassesScheduleParserService
    {
        // The parsing method does too many things, ideally after refactoring a processor class would bind the break-down, validation and building of the entries
        // However the 3 hours are over and I will not refactor it past this point.
        public IEnumerable<LeassessScheduleEntry> Parse(LeassesScheduleDTO leassesScheduleDTO)
        {
            List<LeassessScheduleEntry> scheduleEntries = new List<LeassessScheduleEntry>();

            foreach(var entry in leassesScheduleDTO.ScheduleEntry)
            {
                scheduleEntries.Add(Process(entry.EntryText));
            }

            return scheduleEntries;
        }

        private LeassessScheduleEntry Process(string[] entryText)
        {
            var scheduleEntry = new LeassessScheduleEntry();

            var dateAndPlanSB = new StringBuilder();
            var propertyDescriptionSB = new StringBuilder();
            var dateOfLeaseAndTermSB = new StringBuilder();
            var lesseeTitleSB = new StringBuilder();
            var notes = new List<string>();

            // Minimum one entry is required for processing
            // Update: We skip cancelled items, minimum 2 entries required
            if (entryText.Length > 1)
            {
                (string dateAndPlanRef, string propertyDescription, string dateOfLeaseAndTerm, string lesseeTitle) = ExtractColumnHeadersFromFirstLine(entryText[0]);

                dateAndPlanSB.Append(dateAndPlanRef);
                propertyDescriptionSB.Append(propertyDescription);
                dateOfLeaseAndTermSB.Append(dateOfLeaseAndTerm);
                lesseeTitleSB.Append(lesseeTitle);

                int posEofColumn1 = entryText[0].IndexOf(propertyDescription);
                int posEofColumn2 = entryText[0].IndexOf(dateOfLeaseAndTerm);
                int posEofColumn3 = entryText[0].IndexOf(lesseeTitle);

                for (int index = 1; index < entryText.Length; index++)
                {
                    
                    if(entryText[index] != null)
                    {
                        var r = new Regex("(?:[N|n]ote|NOTE)\\s*\\d*?\\s*?:\\s*(.*)");
                        var result = r.Match(entryText[index]);

                        if (result.Success && !string.IsNullOrWhiteSpace(result.Value))
                        {
                            notes.Add(entryText[index]);
                            continue;
                        }

                        // Appending with whitespace delimiter
                        dateAndPlanSB.Append($" {ExtractTrimmedStringValueBetweenIndexes(entryText[index], 0, posEofColumn1)}");
                        propertyDescriptionSB.Append($" {ExtractTrimmedStringValueBetweenIndexes(entryText[index], posEofColumn1, posEofColumn2)}");
                        dateOfLeaseAndTermSB.Append($" {ExtractTrimmedStringValueBetweenIndexes(entryText[index], posEofColumn2, posEofColumn3)}");
                        lesseeTitleSB.Append($" {ExtractTrimmedStringValueBetweenIndexes(entryText[index], posEofColumn3, entryText[index].Length)}");
                    }
                }
            }

            scheduleEntry.DateAndPlanRef = dateAndPlanSB.ToString();
            scheduleEntry.DateOfLeaseAndTerm = dateOfLeaseAndTermSB.ToString();
            scheduleEntry.PropertyDescription = propertyDescriptionSB.ToString();
            scheduleEntry.LesseeTitle = lesseeTitleSB.ToString();
            scheduleEntry.Notes = notes;

            return scheduleEntry;
        }

        private string ExtractTrimmedStringValueBetweenIndexes(string line, int startIndex, int endIndex)
        {
            // If start index is greater than line's end we return an empty string
            // We do the same if the start index exceeds end index
            // Initially all these checks were defensive against unexpected situations however after a few trial runs it's obvious that they are necessary
            if (startIndex < 0 || startIndex > line.Length + 1 || startIndex >= endIndex || line == null)
            {
                return string.Empty;
            };

            // If the end index is greater than the length of the string we take the value from start to end of line
            if (endIndex > line.Length)
            {
                return line.Substring(startIndex, line.Length - startIndex);
            }

            var length = endIndex - startIndex;
            return line.Substring(startIndex, length).Trim();
        }

        private (string dateAndPlanRef, string propertyDescription, string dateOfLeaseAndTerm, string lesseeTitle) ExtractColumnHeadersFromFirstLine(string line)
        {
            var r = new Regex("^(.{10}\\s*)(.*?(?=\\s{2}))\\S*\\s+(\\S+)\\S*\\s+(\\S+)");

            var result = r.Match(line);

            // Ensure the regex did not fail
            if(result.Groups.Count > 4)
            {
                var dateAndPlanRef = result.Groups[1].Value.Trim();
                var propertyDescription = result.Groups[2].Value.Trim();
                var dateOfLeaseAndTerm = result.Groups[3].Value.Trim();
                var lesseeTitle = result.Groups[4].Value.Trim();

                return (dateAndPlanRef, propertyDescription, dateOfLeaseAndTerm, lesseeTitle);
            } else
            {
                // This scenario requires a different match, not implemented yet
                return (string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }
    }
}
