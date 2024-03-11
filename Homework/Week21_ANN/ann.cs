using System;
using static System.Math;

public class ann{
	public readonly int n; /* number of neurons */
	Func<double,double> f = x => x*Exp(-x*x);	/* activation function */
	public vector p; /* network parameters */
	
	//constructors
	public ann(int m){
		n = m; p = new vector(3*n);
		for(int i=0;i<n;i++){p[3*i]=0;p[3*i+1]=1; p[3*i+2]=1;}
	}

	public ann(int m, Func<double,double> g){
		n = m; p = new vector(n,3); f=g;
                for(int i=0;i<n;i++){p[3*i]=0;p[3*i+1]=1; p[3*i+2]=1;}
	}
	
	//Response
	double response(double x,vector v){
		double res = 0;
		for(int i=0;i<n;i++)res+=v[3*i+2]*f((x-v[3*i])/v[3*i+1]);
		return res;
	}

	public double response(double x){return response(x,p);}

	//train
	public void train_interp(vector x,vector y){
		Func<vector,double> cost = delegate(vector v){double cst = 0; 
			for(int i=0;i<x.size;i++)cst+=Pow(response(x[i],v)-y[i],2); return cst;};
		(vector pmin, double mcst) = min.downhill_sim(cost,p);	
		p = pmin;
	}
}//ann
