using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<double,double,double> f = delegate(double x,double y){return 1;};//Sqrt(1-x*x-y*y);};
		Func<double,double> u = x => Sqrt(1-x*x);
		Func<double,double> d = x => -u(x);

		var res = int2.adint2D(f,-1,1,d,u,0.0001,0.0001);
		WriteLine($"{res.Item1} =?= {2*PI/3}. err = {res.Item2}. #f = {res.Item3}");
		return 0;
	}//Main
}//main
