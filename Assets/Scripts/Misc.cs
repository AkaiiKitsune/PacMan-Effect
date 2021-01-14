using UnityEngine;

public class Misc
{
    public static Vector3 ConvertToMatrixCoordinates(Vector3 coord) => new Vector3(coord.x * LevelDisplayer.size - ((LevelParser.mapWidth * LevelDisplayer.size) / 2), -coord.y * LevelDisplayer.size + ((LevelParser.mapHeight * LevelDisplayer.size) / 2) - LevelDisplayer.size / 2, coord.z - LevelDisplayer.size);

    public static int ConstraintValueBetween(int value, int min, int max) => value > max ? value = max : value < min ? min : value;
}
