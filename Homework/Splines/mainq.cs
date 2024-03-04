using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		int N = 10, M = 100;
                System.Random random = new System.Random();
                vector x = new vector(N), y = new vector(N), z = new double[M];
                for(int i = 0; i < N; i++){
                        x[i] = i+1;
                        y[i] = random.NextDouble();
                        Error.WriteLine($"{x[i]} {y[i]}");}
		interp.qspline qs = new interp.qspline(x,y);
                for(int i = 0; i < M; i++){
                        z[i] = x[0] + i*(x[N-1]-x[0])/(M-1);
                        Out.WriteLine($"{z[i]} {qs.evaluate(z[i])}");}
                return 0;
	}//Main
}//main
