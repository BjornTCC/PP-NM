using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<double,double,double>[] fs = {
					delegate(double x,double y){return Sqrt(1-x*x-y*y);},
					delegate(double x,double y){return Sin(x*y);},
					delegate(double x,double y){return Exp(-x*x)/(y*y);},
					delegate(double x,double y){return Exp(-x*x-y*y);}
					};
		Func<double,double>[] us = {
					x => Sqrt(1-x*x), 
					x => PI/x, 
					delegate(double y){if(y==0)return double.PositiveInfinity;
							   else return 1/(y*y);},
					x => double.PositiveInfinity
					};
		Func<double,double>[] ds = {
					x => -Sqrt(1-x*x),
					x => 0,
					x => 1,
					x => double.NegativeInfinity
					};
		double[] ass = {-1,1, double.NegativeInfinity,double.NegativeInfinity};
		double[] bss = {1,3, double.PositiveInfinity,double.PositiveInfinity};
		double[] exact = {2*PI/3,2*Log(3),Sqrt(PI)/2,PI};

		for(int i=0;i<4;i++){
			var res = int2.integ2D(fs[i],ass[i],bss[i],ds[i],us[i],max_nfev: 99999999);
			WriteLine($"{res.Item1} =?= {exact[i]}.");
		       	WriteLine($"est err = {res.Item2}, exact error = {Abs(res.Item1-exact[i])} nfev = {res.Item3}");
			WriteLine();
		}
		return 0;
	}//Main
}//main
