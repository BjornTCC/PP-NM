using System;
using static System.Console;
using static System.Math;

public class interp{
	public static double linterp(double[] x, double[] y, double z, int i = -1){
		if(i == -1)i = binsearch(x, z); //if statement in case binsearch has been performed previously
		double dx=x[i+1]-x[i]; 
		if(!(dx>0)) throw new Exception("x-Array improperly ordered");
        	double dy=y[i+1]-y[i];
        	return y[i]+dy/dx*(z-x[i]);
	}//linterp
	 
	public static int binsearch(double[] x, double z){
		if(!(x[0] <= z && x[x.Length - 1] >= z)) throw new ArgumentException($"z = {z} outside interval [{x[0]},{x[x.Length-1]}].");
		int i = 0, j = x.Length-1; //endpoints
		while(j-i>1){ 
			int mid=(i+j)/2;
			if(z>x[mid]) i=mid; else j=mid;
		}
		return i;
	}//binsearch
	
	public static double linterpInt(double[] x, double[] y, double z){
		int i = binsearch(x, z);
		double res = 0;
		for(int j = 0; j < i; j++)res += (x[j+1]-x[j])*(y[j+1]+y[j])/2; //add the integrals of each triangle/trapezoid rule
		res += (z-x[i])*(linterp(x,y,z,i) + y[i])/2;
		return res;
	}//linterpInt
	
	public class qspline{
		public vector x,y,b,c;
		public qspline(vector xs,vector ys){
			if(xs.size != ys.size) throw new ArgumentException($"Data sizes incompatible x: {xs.size}, y:{ys.size}");
			x=xs.copy(); y=ys.copy();
			int n = x.size;
		     	vector p = new vector(n-1), dx = new vector(n-1);
			b = new vector(n-1); 
			c = new vector(n-1);
			for(int i = 0; i < n-1; i++){
				dx[i] = x[i+1] - x[i];
				p[i] = (y[i+1] - y[i])/dx[i];}
			for(int i = 0; i < n-2; i++)c[i+1] =(p[i+1] - p[i] -c[i]*dx[i])/dx[i+1];
			c[n-2] /= 2;
			for(int j = n-3; j >= 0; j--)c[j] = (p[j+1] - p[j] -c[j+1]*dx[j+1])/dx[j];
			for(int i = 0; i < n-1; i++)b[i] = p[i] - c[i]*dx[i];
		}//constructor
		
		public double evaluate(double z){
			int i = binsearch(x, z);
			double res = y[i] + b[i]*(z - x[i]) + c[i]*(z-x[i])*(z - x[i]);
			return res;
		}//evaluate

		public double derivative(double z){
			int i = binsearch(x, z);
			double res = b[i] + 2*c[i]*(z-x[i]); 
			return res;
		}//derivative

		public double integral(double z){
			int i=binsearch(x,z);
			double res = 0;
			for(int j = 0; j < i; j++){
				res += (x[j+1] - x[j])*y[j] + b[j]*Pow(x[j+1]-x[j],2)/2 + c[i]*Pow(x[j+1] - x[j],3)/3;}
			res+= (z - x[i])*y[i] + b[i]*Pow(z-x[i],2)/2 + c[i]*Pow(z - x[i],3)/3;
			return res;	
		}//integral
	}//qspline
}//spline
