using static System.Math;
using System;

public static class int2{

	public static (double,double,int) integ2D(
			Func<double,double,double> f,
			double a, double b,
			Func<double,double> d,
			Func<double,double> u,
			double acc = 0.001, 
			double eps = 0.001,
			int max_nfev = 9999){
		
		int f_eval = 0;
		Func<double,double,double> f_e = delegate(double x, double y){
			if(f_eval < max_nfev){
				f_eval++;
				return f(x,y);}
			else{
				throw new OperationCanceledException($"integ2D max_fev of {max_nfev} reached.");
			}
		};	
		Func<double,double> F = delegate(double x){
					Func<double,double> g = y => f_e(x,y);
					return integrate.integral(g,d(x),u(x),acc/Sqrt(b-a),eps).Item1;
					};
		var res = integrate.integral(F,a,b,acc,eps);
		return(res.Item1,res.Item2,f_eval);
	}//adint2D
}//int2
