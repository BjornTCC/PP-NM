using System;
using static System.Console;
using static System.Math;

class main{
	public static void Main(){
		Func<double,double> f = x => x*Exp(-x);
		Func<double,double> g = x => Sin(x);
		Func<double,double> F = s => 1/Pow(s+1,2);
		Func<double,double> G = s => 1/(s*s+1);

		Func<double,double> fint = delegate(double s){
			Func<double,double,double> k = delegate(double t,double u){
				return g(u)*f(t-u)*Exp(-s*t);
			};
			Func<double,double> up = x => x;
			Func<double,double> d = x => 0;
			double a = 0;
			double b = double.PositiveInfinity;
			return int2.integ2D(k,a,b,d,up,0.001,0.001,9999999).Item1;
		};
		int N = 200;
		double smin = 1;
		double smax = 3;
		for(int i=0;i<N;i++){
		 	double s = smin + (smax-smin)*i/N;
		 	WriteLine($"{s} {F(s)*G(s)} {fint(s)}");
		}
	}//Main
}//main
