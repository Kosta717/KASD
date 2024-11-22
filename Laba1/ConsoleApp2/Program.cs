using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string VectorF = "vector.txt";

        StreamReader streamReader = new StreamReader(VectorF);
        int dimen = int.Parse(streamReader.ReadLine());
        double[][] matr = new double[dimen][];
        int k = 0;
        while (k < dimen)
        {
            matr[k] = streamReader.ReadLine().Split(' ').Select(x => double.Parse(x)).ToArray();
            k++;
        }

        double[] vector = null;

        if (Simm(matr) == true)
        {
            vector = streamReader.ReadLine().Split(' ').Select(x => double.Parse(x)).ToArray();
        }
        else
        {
            Console.WriteLine("Матрица несимметрична.");
            return; 
        }

        double length = CalcVectorLength(vector, matr);
        Console.WriteLine($"Длина вектора: {length}");
    }

    public static bool Simm(double[][] matr)
    {
        int f = 1; 

        for (int i = 0; i < matr.Length; i++)
        {
            for (int j = 0; j < matr[i].Length; j++)
            {
                if (matr[i][j] != matr[j][i])
                {
                    f = 0; 
                }
            }
        }

        return f == 1; 
    }

    public static double CalcVectorLength(double[] vector, double[][] matr)
    {
        double sum1 = 0;

        for (int i = 0; i < vector.Length; i++)
        {
            double sum2 = 0;
            for (int j = 0; j < vector.Length; j++)
            {
                sum2 += matr[i][j] * vector[j];
            }
            sum1 += vector[i] * sum2;
        }

        return Math.Sqrt(sum1);
    }
}
