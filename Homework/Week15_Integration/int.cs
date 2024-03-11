using System;
using static System.Math;

public static class integrate{
	
	public static double[] adint(Func<double,double> f, double a, double b, 	//returns in format (val, est. err., #eval)
	double delta=0.001, double eps=0.001, double f2=double.NaN, double f3=double.NaN){
		double h=b-a;  
		double evals = 0;
		if(Double.IsNaN(f2)){ f2=f(a+2*h/6); f3=f(a+4*h/6); evals+=2; } // first call, no points to reuse
		double f1=f(a+h/6), f4=f(a+5*h/6);
		evals +=2; //Add the intagrand evaluations
		double Q = (2*f1+f2+f3+2*f4)/6*(b-a); // higher order rule
		double q = (  f1+f2+f3+  f4)/4*(b-a); // lower order rule
		double err = Abs(Q-q);
		if (err <= delta + eps*Abs(Q)){
			double[] res = new double[3]{Q,err,evals};
			return res;}
		else{ 
			double[] res1 = adint(f,a,(a+b)/2,delta/Sqrt(2),eps,f1,f2);
			double[] res2 = adint(f,(a+b)/2,b,delta/Sqrt(2),eps,f3,f4); //Recursively split the interval 
			evals += res1[2] + res2[2];
			err = Sqrt(res1[1]*res1[1] + res2[1]*res2[1]);
			double[] res = new double[3]{res1[0] + res2[0], err, evals};
			return res;}
	}//adint
	
	public static double[] transint(Func<double,double> f, double a, double b,
        double delta=0.001, double eps=0.001){
		Func<double,double> ft = delegate(double theta){return f((a+b)/2 + Cos(theta)*(b-a)/2)*Sin(theta)*(b-a)/2;};
		return adint(ft, 0, PI, delta, eps);
	}//transint
	
	public static double[] integral(Func<double,double> f, double a, double b,         //returns in format (val, est. err., #eval)
        double delta=0.001, double eps=0.001, double f2=double.NaN, double f3=double.NaN){
		if(double.IsPositiveInfinity(b) && double.IsNegativeInfinity(a)){
			Func<double,double> fs = t=> f(t/(1-t*t))*(1+t*t)/Pow(1-t*t,2);
			return adint(fs,-1,1,delta,eps);}
		if(double.IsPositiveInfinity(b)){
			Func<double,double> fs = t=>f(a + (1-t)/t)/(t*t);
			return adint(fs,0,1,delta,eps);}
		if(double.IsNegativeInfinity(a)){
			Func<double,double> fs = t=>f(b-(1-t)/t)/(t*t);
			return adint(fs,0,1);}
		return adint(f,a,b,delta,eps);
	}//integral

}//integrate
