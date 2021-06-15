using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Paths
{
    Dictionary<Tuple<string, string>, List<Tuple<double, double>>> allWays = new Dictionary<Tuple<string, string>, List<Tuple<double, double>>>();
    List<String> cities = new List<string>{ "Novgorod", "Riga", "Toropets" };
    public Paths() 
    {
        addPath("Novgorod", "Riga", pushPointToPath(2.43, 1.8, 0.6, 0.33, -1.13, -3.45, -2.58, -4.36, -4.44, -4.08, -6.48, -6.12, -8.4, -6.65, - 10.93, -5.29));
        addPath("Riga", "Novgorod", pushPointToPath(-10.93, -5.29, -8.4, -6.65, -6.48, -6.12, -4.44, -4.08, -2.58, -4.36, -1.13, -3.45, 0.6, 0.33, 2.43, 1.8));
        addPath("Riga", "Toropets", pushPointToPath(-10.93, -5.29, -8.51, -6.72, -4.94, -5.48, -2.95, -7.11, -1.6, -9.07, 2.28, -9.27, 3.49, -7.9, 4.19, -6.33));
        addPath("Toropets", "Riga", pushPointToPath(4.19, -6.33, 3.49, -7.9, 2.28, -9.27, -1.6, -9.07, -2.95, -7.11, -4.94, -5.48, -8.51, -6.72, -10.93, -5.29));
        addPath("Toropets", "Novgorod", pushPointToPath(4.19, -6.33, 3.09, -7.52, 1.05, -6.65, 2.0, -4.4, -0.99, -3.63, -0.56, -1.21, 0.97, 0.68, 2.43, 1.8));
        addPath("Novgorod", "Toropets", pushPointToPath(2.43, 1.8, 0.97, 0.68, -0.56, -1.21, -0.99, -3.63, 2.0, -4.4, 1.05, -6.65, 3.09, -7.52, 4.19, -6.33));
    }
    private void addPath(string city1, string city2, List<Tuple<double, double>> path) 
    {
        allWays.Add(new Tuple<string, string>(city1, city2), path);
    }
    
    private List<Tuple<double, double>> pushPointToPath(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, 
                                 double x5, double y5, double x6, double y6, double x7, double y7, double x8, double y8)
    {
        List<Tuple<double, double>> pathFromCity1ToCity2 = new List<Tuple<double, double>>();
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x1, y1));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x2, y2));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x3, y3));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x4, y4));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x5, y5));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x6, y6));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x7, y7));
        pathFromCity1ToCity2.Add(new Tuple<double, double>(x8, y8));

        return pathFromCity1ToCity2;
    }

    public List<Tuple<double, double>> getWay(string city1, string city2)
    {
        if (cities.Contains(city1) && cities.Contains(city2))
        {
            return allWays[new Tuple<string, string>(city1, city2)];
        }
        else 
        {
            return null;
        }
    }
}