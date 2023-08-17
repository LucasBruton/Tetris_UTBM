using System.Collections.Generic;
using UnityEngine;

// Class used to store data for the sector levels
public static class SectorData {
    // First sector chosen by the player
    public static Sector firstSector{ get; private set; }
    // Second sector chosen by the player
    public static Sector secondSector{ get; private set; }

    // Next sector level
    public static int nextSector{ get; private set; } = 1;

    // Number of CSs completed by the player during the first computer science year level
    public static Dictionary<CSYear1UVType, int> NumberOfCSsCompleted { get; private set; } = new Dictionary<CSYear1UVType, int>() {
        {CSYear1UVType.AlgoAndPrograming, 0},
        {CSYear1UVType.Network, 0},
        {CSYear1UVType.HardwareAndSystems, 0},
        {CSYear1UVType.Database, 0},
        {CSYear1UVType.TheoreticalComputerScience, 0},
    };

    // Set the first sector chosen by the player
    public static void SetFirstSector(Sector sector) {
        firstSector = sector;
        nextSector = 1;
    }

    // Set the second sector chosen by the player
    public static void SetSecondSector(Sector sector) {
        secondSector = sector;
        nextSector = 2;
    }

    // Set the number of completed CSs of a type of CS
    public static void CompletedCS(CSYear1UVType csCompleted, int nbOfCompleted) {
        NumberOfCSsCompleted[csCompleted] = nbOfCompleted;
    }

    // Reset the data of the class
    public static void ResetSectorData() {
        NumberOfCSsCompleted = new Dictionary<CSYear1UVType, int>() {
            {CSYear1UVType.AlgoAndPrograming, 0},
            {CSYear1UVType.Network, 0},
            {CSYear1UVType.HardwareAndSystems, 0},
            {CSYear1UVType.Database, 0},
            {CSYear1UVType.TheoreticalComputerScience, 0},
        };
        nextSector = 1;
    }

    // Returns the current sector
    public static Sector getCurrentSector() {
        if(nextSector == 1) {
            return firstSector;
        }

        return secondSector;
    }
}