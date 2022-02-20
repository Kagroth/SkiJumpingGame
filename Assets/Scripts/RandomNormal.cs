using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Marsaglia polar method in Java and C#
More here: https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/

##### Java ####################
private static double spare;
private static boolean hasSpare = false;

public static synchronized double generateGaussian(double mean, double stdDev) {
    if (hasSpare) {
        hasSpare = false;
        return spare * stdDev + mean;
    } else {
        double u, v, s;
        do {
            u = Math.random() * 2 - 1;
            v = Math.random() * 2 - 1;
            s = u * u + v * v;
        } while (s >= 1 || s == 0);
        s = Math.sqrt(-2.0 * Math.log(s) / s);
        spare = v * s;
        hasSpare = true;
        return mean + stdDev * u * s;
    }
}

###########################
#### C#
public static float NextGaussian() {
    float v1, v2, s;
    do {
        v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
        v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
        s = v1 * v1 + v2 * v2;
    } while (s >= 1.0f || s == 0f);

    s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
 
    return v1 * s;
}
*/
public class RandomNormal
{
    public static float NextFloat(float mean, float std) {
        float u, v, s;

        do {
            u = Random.Range(0f, 1f) - 1.0f;
            v = Random.Range(0f, 1f) - 1.0f;
            s = u * u + v * v;
        } while (s >= 1 || s == 0);

        s = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);

        return mean + std * u * s;
    }
}
