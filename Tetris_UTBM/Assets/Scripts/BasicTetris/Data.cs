using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumeration of the scenes of the game
public enum Scene {
    TC1Rules, TC11, TC12, TC13, TC14, TC2, TC2Rules, ComputerScienceYear1, ComputerScienceYear1Rules, GameOver, MainMenu, Sector, SectorRules, Sector1stChoice, Sector2ndChoice, FinishedGame
}

// Enumeration of the Computer Science Sectors 
public enum Sector {
    DataScience, SoftwareDeployment, VirtualWorlds, EmbeddedSystems, Networks, AdvancedITDevelopment, Cybersecurity, AI, ArtificialVision
}

// Data used by the scripts of the game
public static class Data
{
    public static readonly Dictionary<Tetramino, Vector2Int[]> Cells = new Dictionary<Tetramino, Vector2Int[]>()
    {
        { Tetramino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) } },
        { Tetramino.J, new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 2) } },
        { Tetramino.L, new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 2) } },
        { Tetramino.O, new Vector2Int[] { new Vector2Int(1, 2), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
        { Tetramino.S, new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(2, 2) } },
        { Tetramino.T, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2) } },
        { Tetramino.Z, new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(1, 1), new Vector2Int(2, 1) } },
    };

    public static readonly Dictionary<Tetramino, Vector2Int[]> Rotation = new Dictionary<Tetramino, Vector2Int[]>()
    {
        { Tetramino.I, new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(0,-1) } },
        { Tetramino.J, new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(2, 2), new Vector2Int(2, 0), new Vector2Int(0, 0) } },
        { Tetramino.L, new Vector2Int[] { new Vector2Int(2, 2), new Vector2Int(2, 0), new Vector2Int(0, 0), new Vector2Int(0, 2) } },
        { Tetramino.T, new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1,1) } },
        { Tetramino.O, new Vector2Int[] { new Vector2Int(1, 2), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(1, 1) } },
        { Tetramino.S, new Vector2Int[] { new Vector2Int(1, 2), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(2, 0) } },
        { Tetramino.Z, new Vector2Int[] { new Vector2Int(2, 2), new Vector2Int(2, 1), new Vector2Int(1, 1), new Vector2Int(1, 0) } },
    };

    public static readonly Vector2Int[] Horizontal = new Vector2Int[]
    { 
        new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(-3, -3)
    };

    public static readonly Vector2Int[] Vertical = new Vector2Int[]
    {
        new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(-3, -3)
    };

    // Dictionnary that links a string to the scenes of the game
    public static readonly Dictionary<Scene, string> SceneNames = new Dictionary<Scene, string>() {
        {Scene.TC1Rules, "TC1 - règles"},
        {Scene.TC11, "TC1 - Niveau 1"},
        {Scene.TC12, "TC1 - Niveau 2"},
        {Scene.TC2, "TC2"},
        {Scene.TC2Rules, "TC2 - règles"},
        {Scene.ComputerScienceYear1, "ComputerScienceYear1"},
        {Scene.ComputerScienceYear1Rules, "ComputerScienceYear1 - règles"},
        {Scene.GameOver, "GameOver"},
        {Scene.MainMenu, "MainMenu"},
        {Scene.Sector, "Filière"},
        {Scene.SectorRules, "Filière - Règles"},
        {Scene.Sector1stChoice, "Filière - choix 1e filière"},
        {Scene.Sector2ndChoice, "Filière - choix 2e filière"},
        {Scene.FinishedGame, "FinishedGame"}
    };

    // Dictionnary that links a string to the Computer Science sectors 
    public static readonly Dictionary<Sector, string> SectorNames = new Dictionary<Sector, string>() {
        {Sector.DataScience, "Data Science"},
        {Sector.SoftwareDeployment, "Déploiement logiciel"},
        {Sector.VirtualWorlds, "Mondes Virtuels"},
        {Sector.EmbeddedSystems, "Robotique et Systèmes Embarqués"},
        {Sector.Networks, "Conception et déploiement réseaux"},
        {Sector.AdvancedITDevelopment, "Développement Informatique Avancé"},
        {Sector.Cybersecurity, "Sécurité et Virtualisation de l'infrastructure réseau"},
        {Sector.AI, "Intelligence Artificielle"},
        {Sector.ArtificialVision, "Vision Artificielle"}
    };

}
