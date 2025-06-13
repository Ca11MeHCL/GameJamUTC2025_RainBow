using UnityEngine;

public struct EGameOver
{

}

/// <summary>
/// When player earn score
/// </summary>
public struct EEarnScore
{

}

/// <summary>
/// When an enemy die
/// </summary>
public struct EEnemyDie
{
    public Vector3 position;

    public EEnemyDie(Vector3 pos)
    {
        position = pos;
    }
}
/// <summary>
/// When data changed
/// </summary>
public struct EEndLevel
{
    public int level;
}
