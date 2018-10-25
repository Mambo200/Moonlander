using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VektorenFormativ;
static public class CubeEckPosition
{
    /// <summary>
    /// Berechnet die Eckpunkte eines Würfels
    /// </summary>
    /// <param name="_cube">Würfel</param>
    /// <returns>Eckpunkte des Würfels</returns>
    static public Vector[] GetCubeVectorPosition(GameObject _cube)
    {
        Vector[] v = new Vector[8];

        // get Scale of cube
        Vector cubeScale = new Vector(_cube.transform.lossyScale.x, _cube.transform.lossyScale.y, _cube.transform.lossyScale.z);

        // get Position of Cube
        Vector cubePosition = new Vector(_cube.transform.position.x, _cube.transform.position.y, _cube.transform.position.z);

        // get Rotation of Cube
        Vector cubeRotation = new Vector(_cube.transform.rotation.eulerAngles.x, _cube.transform.rotation.eulerAngles.y, _cube.transform.rotation.eulerAngles.z);

        v[0] = new Vector(
            cubePosition.X + -(cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + -(cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + -(cubeScale.Z / 2)/* + cubeRotation.Z*/
          );
        v[1] = new Vector(
            cubePosition.X + (cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + -(cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + -(cubeScale.Z / 2)/* + cubeRotation.Z*/
            );
        v[2] = new Vector(
            cubePosition.X + (cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + (cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + -(cubeScale.Z / 2)/* + cubeRotation.Z*/
            );
        v[3] = new Vector(
            cubePosition.X + -(cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + (cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + -(cubeScale.Z / 2)/* + cubeRotation.Z*/
            );

        v[4] = new Vector(
            cubePosition.X + -(cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + -(cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + (cubeScale.Z / 2)/* + cubeRotation.Z*/
            );
        v[5] = new Vector(
            cubePosition.X + (cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + -(cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + (cubeScale.Z / 2)/* + cubeRotation.Z*/
            );
        v[6] = new Vector(
            cubePosition.X + (cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + (cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + (cubeScale.Z / 2)/* + cubeRotation.Z*/
            );
        v[7] = new Vector(
            cubePosition.X + -(cubeScale.X / 2)/* + cubeRotation.X*/,
            cubePosition.Y + (cubeScale.Y / 2)/* + cubeRotation.Y*/,
            cubePosition.Z + (cubeScale.Z / 2)/* + cubeRotation.Z*/
            );

        return v;
    }
}
