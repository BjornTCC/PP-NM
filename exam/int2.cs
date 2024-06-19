using static System.Math;
using System;

public static class int2{

	public static (double,double,int) adint2D(
			Func<double,double,double> f,
			double a, double b,
			Func<double,double> d,
			Func<double,double> u,
			double acc = 0.001, 
			double eps = 0.001){
		
		int f_eval = 0;
		Func<double,double,double> f_e = delegate(double x, double y){f_eval++;return f(x,y);};	
		Func<double,double> F = delegate(double x){
					Func<double,double> g = y => f_e(x,y);
					return integrate.adint(g,d(x),u(x),acc/Sqrt(b-a),eps).Item1;
					};
		var res = integrate.adint(F,a,b,acc,eps);
		return(res.Item1,res.Item2,f_eval);
	}//adint2D
}//int2
