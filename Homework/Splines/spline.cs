using System;
using System.Console;
using static System.Math;

public class interp{
	public static double linterp(double[] x, double[] y, double z){
		int i = binsearch(x, z);
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
		for(int j = 0; j < i-1; j++){
			double dx = x[j+1]-x[j];
			if(!(dx>0)) throw new Exception("x-Array improperly ordered");
			res += dx*(y[j+1]+y[j])/2;
		}
		res += (z-x[i])*(linterp(x,y,z) + y[i])/2;
		return res;
	}//linterpInt
	
	public class qspline{
		vector x,y,b,c;
		public qspline(vector xs,vector ys){
			if(xs.size != ys.size) throw new ArgumentException($"Data sizes incompatible x: {xs.size}, y:{ys.size}");
			x=xs.copy(); y=ys.copy();
			int n = x.size;
		     	double [] p = double[n-1], dx = double[n-1];
			for(int i = 0; i < n-1; i++){
				dx[i] = x[i+1] - x[i];
				p[i] = (y[i+1] - y[i])/dx[i];
			for(int j = 0; j < n-2; j++)c[i] =(p[i+1] - p[i] -c[i]*dx[i])/dx[i+1];
			c[n-2]/=2;
			for(int j = n-3; j >= 0; j--)c[j] = (p[j+1] - p[j] -c[j+1]*dx[j+1])/dx[j];
			for(int i = 0; i < n-1; i++)b[i] = p[i] - c[i]*dx[i];
		}//constructor


	}//qspline
}//spline
