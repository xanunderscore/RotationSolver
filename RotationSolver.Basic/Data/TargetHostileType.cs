namespace RotationSolver.Basic.Data;

/// <summary>
/// Hostile target.
/// </summary>
public enum TargetHostileType : byte
{
    /// <summary>
    /// All enemies
    /// </summary>
    All,

    /// <summary>
    /// Hostile enemies, or all enemies if none are hostile
    /// </summary>
    HostileOrAll,

    /// <summary>
    /// Hostile enemies
    /// </summary>
    Hostile,

    /// <summary>
    /// Hostile enemies targeting party members
    /// </summary>
    HostileParty,
}
