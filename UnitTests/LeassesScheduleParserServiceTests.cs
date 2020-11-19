using OrbitalWitness.Models;
using OrbitalWitness.Models.DTO;
using OrbitalWitness.Services;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class LeassesScheduleParserServiceTests
    {
        private LeassesScheduleParserService _subjectUnderTest;

        [Fact]
        public void Parse_NoNotesScenario_ReturnAListOfParsedEntries()
        {
            // Arrange
            _subjectUnderTest = new LeassesScheduleParserService();

            var leassesScheduleDTOSample = new LeassesScheduleDTO
            {
                ScheduleType = "SCHEDULE OF NOTICES OF LEASE",
                ScheduleEntry = new List<ScheduleEntryDTO>
                {
                    new ScheduleEntryDTO
                    {
                        EntryNumber = "1",
                        EntryDate = "",
                        EntryType = "Schedule of Notices of Leases",
                        EntryText = new string[]
                        {
                            "28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039  ",
                            "tinted blue     Floor)                        99 years from              ",
                            "(part of)                                     23.1.2009"
                        }
                    },
                    new ScheduleEntryDTO
                    {
                        EntryNumber = "1",
                        EntryDate = "",
                        EntryType = "Schedule of Notices of Leases",
                        EntryText = new string[]
                        {
                            "09.07.2009      Endeavour House, 47 Cuba      06.07.2009      EGL557357  ",
                            "Edged and       Street, London                125 years from             ",
                            "numbered 2 in                                 1.1.2009                   ",
                            "blue (part of)"
                        }
                    },
                }
            };

            var expectedLeassessScheduleEntries = new List<LeassessScheduleEntry>
            {
                new LeassessScheduleEntry
                {

                    DateAndPlanRef = "28.01.2009 tinted blue (part of)",
                    PropertyDescription = "Transformer Chamber (Ground Floor) ",
                    DateOfLeaseAndTerm = "23.01.2009 99 years from 23.1.2009",
                    LesseeTitle = "EGL551039  "
                },
                new LeassessScheduleEntry
                {
                    DateAndPlanRef = "09.07.2009 Edged and numbered 2 in blue (part of)",
                    PropertyDescription = "Endeavour House, 47 Cuba Street, London  ",
                    DateOfLeaseAndTerm = "06.07.2009 125 years from 1.1.2009 ",
                    LesseeTitle = "EGL557357  "
                }
            };

            // Act
            var result = _subjectUnderTest.Parse(leassesScheduleDTOSample);

            // Assert
            AssertOutputMatchesExpected(result, expectedLeassessScheduleEntries);
        }

        [Fact]
        public void Parse_WithNotesScenario_ReturnAListOfParsedEntries()
        {
            // Arrange
            _subjectUnderTest = new LeassesScheduleParserService();

            var leassesScheduleDTOWithNotesSample = new LeassesScheduleDTO
            {
                ScheduleType = "SCHEDULE OF NOTICES OF LEASE",
                ScheduleEntry = new List<ScheduleEntryDTO>
                {
                    new ScheduleEntryDTO
                    {
                        EntryNumber = "1",
                        EntryDate = "",
                        EntryType = "Schedule of Notices of Leases",
                        EntryText = new string[]
                        {
                            "28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039  ",
                            "tinted blue     Floor)                        99 years from              ",
                            "(part of)                                     23.1.2009",
                            "NOTE 1: The Lease comprises also other land.",
                            null,
                            "NOTE 2: Copy Lease filed under SY76788."
                        }
                    },
                }
            };

            var expectedLeassesScheduleDTOWithNotesEntries = new List<LeassessScheduleEntry>
            {
                new LeassessScheduleEntry
                {

                    DateAndPlanRef = "28.01.2009 tinted blue (part of)",
                    PropertyDescription = "Transformer Chamber (Ground Floor) ",
                    DateOfLeaseAndTerm = "23.01.2009 99 years from 23.1.2009",
                    LesseeTitle = "EGL551039  ",
                    Notes = new List<string>
                    {
                        "NOTE 1: The Lease comprises also other land.",
                        "NOTE 2: Copy Lease filed under SY76788."
                    }
                },
            };

            // Act
            var result = _subjectUnderTest.Parse(leassesScheduleDTOWithNotesSample);

            // Assert
            AssertOutputMatchesExpected(result, expectedLeassesScheduleDTOWithNotesEntries);
        }

        private void AssertOutputMatchesExpected(IEnumerable<LeassessScheduleEntry> result, List<LeassessScheduleEntry> expectedLeassessScheduleEntries)
        {
            result.Count().ShouldBe(expectedLeassessScheduleEntries.Count);

            for(int i = 0; i < expectedLeassessScheduleEntries.Count; i++)
            {
                AssertResultIsExpected(result.ToList()[i], expectedLeassessScheduleEntries[i]);
            }
        }

        private void AssertResultIsExpected(LeassessScheduleEntry result, LeassessScheduleEntry expected)
        {
            result.DateAndPlanRef.Trim().ShouldBe(expected.DateAndPlanRef.Trim());
            result.PropertyDescription.Trim().ShouldBe(expected.PropertyDescription.Trim());
            result.DateOfLeaseAndTerm.Trim().ShouldBe(expected.DateOfLeaseAndTerm.Trim());
            result.LesseeTitle.Trim().ShouldBe(expected.LesseeTitle.Trim());
            result.Notes.ShouldNotBeNull(); // Can be empty list but not null

            if (expected.Notes != null && expected.Notes.Any())
            {
                result.Notes.Count().ShouldBe(expected.Notes.Count());

                foreach(var note in result.Notes)
                {
                    expected.Notes.Contains(note).ShouldBeTrue();
                }
            }
        }
    }
}
