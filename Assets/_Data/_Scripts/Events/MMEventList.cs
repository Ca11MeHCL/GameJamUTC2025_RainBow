using UnityEngine;

public struct EGameOver
{

}

/// <summary>
/// When player earn score
/// </summary>
public struct EDataChanged
{

}

/// <summary>
/// When an enemy die
/// </summary>
public struct EEnemyDie
{
    public Vector3 position;
    public int score, energy;
    public EEnemyDie(Vector3 pos)
    {
        position = pos;
        score = 0;
        energy = 0;
    }

    public EEnemyDie(Vector3 pos, int score, int energy)
    {
        position = pos;
        this.score = score;
        this.energy = energy;
    }
}
/// <summary>
/// When data changed
/// </summary>
public struct EEndLevel
{
    public int level;
}
