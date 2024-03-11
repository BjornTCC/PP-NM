using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<double,double> f1 = x => Pow(x, -0.5);
		Func<double,double> f2 = x => Log(x)/Sqrt(x);
		double[] Res1 = integrate.transint(f1,0,1,1e-5);
                double[] Res2 = integrate.transint(f2,0,1,1e-5);
		WriteLine($"1: Analytic: 2, Numerical: {Res1[0]}, err: {Res1[1]}, #evals: {Res1[2]}");
                WriteLine($"1: Analytic: -4, Numerical: {Res2[0]}, err: {Res2[1]}, #evals: {Res2[2]}");
	        return 0;	
	}//Main
}//main
