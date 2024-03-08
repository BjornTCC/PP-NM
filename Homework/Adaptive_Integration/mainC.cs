using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<double,double>[] fs = new Func<double,double>[4]{x => Exp(-x*x), x=> Exp(x), x=> Pow(x,4)*Exp(-x), x=> Pow(x,9)*Exp(-x)};
		string[] names = new string[4]{"Gaussian", "Exponential", "Gamma(5)", "Gamma(10)"};
	      	double[] vals = new double[4]{Sqrt(PI), E, 24, 362880};
		double[] a = new double[4]{double.NegativeInfinity, double.NegativeInfinity, 0, 0};
		double[] b = new double[4]{double.PositiveInfinity, 1, double.PositiveInfinity, double.PositiveInfinity};
		for(int i = 0; i < 4; i++){
			double[] res = integrate.integral(fs[i],a[i],b[i]);
			WriteLine($"{names[i]}: Exact: {vals[i]}, Numerical: {res[0]}, err: {res[1]}, #eval: {res[2]}");
		}	
	        return 0;	
	}//Main
}//main
