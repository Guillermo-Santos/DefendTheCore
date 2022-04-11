using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Sort the array on random
    /// </summary>
    /// <typeparam name="T"> type of the array </typeparam>
    /// <param name="array"> array of data to sort </param>
    /// <param name="seed"> seed for randomizer </param>
    /// <returns></returns>
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random random = new System.Random(seed);

        for(int i = 0; i < array.Length-1; i++)
        {
            int randomIndex = random.Next(i,array.Length-1);
            T result = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = result;
        }

        return array;
    }
    /// <summary>
    /// Rotate the GameObject to look on target
    /// </summary>
    /// <param name="obj"> Is the transform of the GameObject to be rotated</param>
    /// <param name="Direction">Is the direction the transform has to look</param>
    /// <param name="speed">Is the speed of the rotation</param>
    public static void LookOnTarget(Transform obj, Vector3 Direction, float speed)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Direction.normalized);
        Vector3 rotation = Quaternion.Lerp(obj.rotation, lookRotation, Time.deltaTime * speed).eulerAngles;
        obj.rotation = Quaternion.Euler(rotation.x, rotation.y, obj.rotation.z);
    }


}
