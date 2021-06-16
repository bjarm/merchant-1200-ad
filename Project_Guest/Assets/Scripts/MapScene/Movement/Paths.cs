using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Paths
{
    Dictionary<Tuple<string, string>, List<Tuple<double, double>>> allWays = new Dictionary<Tuple<string, string>, List<Tuple<double, double>>>();
    List<String> cities = new List<string>{ "Novgorod", "Riga", "Toropets", "Pskov", "Polotsk", "Vitebsk" };
    public Paths() 
    {
        addPath("Novgorod", "Pskov", pushPointToPath(2.43, 1.8, 1.32, 0.85, 0.42, 0.01, -0.43, -0.84, -0.86, -2.06, -0.59, -3.27, -1.2, -3.44, -2.19, -2.94));
        addPath("Pskov", "Novgorod", pushPointToPath(-2.19, -2.94, -1.2, -3.44, -0.59, -3.27, -0.86, -2.06, -0.43, -0.84, 0.42, 0.01, 1.32, 0.85, 2.43, 1.8));
        addPath("Pskov", "Riga", pushPointToPath(-2.19, -2.94, -1.15, -3.51, -2.48, -4.32, -4.3, -3.93, -4.72, -5.43, -7.16, -6.45, -10.12, -6.68, -10.93, -5.29));
        addPath("Riga", "Pskov", pushPointToPath(-10.93, -5.29, -10.12, -6.68, -7.16, -6.45, -4.72, -5.43, -4.3, -3.93, -2.48, -4.32, -1.15, -3.51, -2.19, -2.94));
        addPath("Pskov", "Polotsk", pushPointToPath(-2.19, -2.94, -1.15, -3.51, 0.28, -4.52, 2.18, -4.38, 0.96, -6.34, -0.03, -7.86, -1.83, -9.24, -2.75, -9.86));
        addPath("Polotsk", "Pskov", pushPointToPath(-2.75, -9.86, -1.83, -9.24, -0.03, -7.86, 0.96, -6.34, 2.18, -4.38, 0.28, -4.52, -1.15, -3.51, -2.19, -2.94));
        addPath("Polotsk", "Toropets", pushPointToPath(-2.75, -9.86, -0.97, -8.89, 2.74, -9.21, 3.34, -8.74, 5.6, -9.43, 6.49, -7.61, 5.57, -7.06, 4.19, -6.33));
        addPath("Toropets", "Polotsk", pushPointToPath(4.19, -6.33, 5.57, -7.06, 6.49, -7.61, 5.6, -9.43, 3.34, -8.74, 2.74, -9.21, -0.97, -8.89, -2.75, -9.86));
        addPath("Polotsk", "Vitebsk", pushPointToPath(-2.75, -9.86, -0.97, -8.89, 2.74, -9.21, 3.34, -8.74, 5.6, -9.43, 6.38, -10.11, 5.46, -11.78, 2.88, -11.94));
        addPath("Vitebsk", "Polotsk", pushPointToPath(2.88, -11.94, 5.46, -11.78, 6.38, -10.11, 5.6, -9.43, 3.34, -8.74, 2.74, -9.21, -0.97, -8.89, -2.75, -9.86));
        addPath("Toropets", "Vitebsk", pushPointToPath(4.19, -6.33, 5.5, -6.99, 6.4, -7.52, 6.17, -8.41, 5.6, -9.43, 6.38, -10.11, 5.46, -11.78, 2.88, -11.94));
        addPath("Vitebsk", "Toropets", pushPointToPath(2.88, -11.94, 5.46, -11.78, 6.38, -10.11, 5.6, -9.43, 6.17, -8.41, 6.4, -7.52, 5.5, -6.99, 4.19, -6.33));
        
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
        if (allWays.ContainsKey(new Tuple<string, string>(city1, city2)))
        {
            return allWays[new Tuple<string, string>(city1, city2)];
        }
        else 
        {
            return new List<Tuple<double, double>>();
        }
    }
}