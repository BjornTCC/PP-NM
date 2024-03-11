using System;
using static System.Console;
using static System.Math;

public class main{

	public static int Main(string[] args){
		double rmin = 1e-4, rmax = 8, acc = 0.001, eps = 0.001;
		vector fmin = new vector(2);
		vector fm = new vector(1);
		fmin[0] = rmin*(1-rmin);
		fm[0] = rmin*(1-rmin);
		fmin[1] = 1-2*rmin;
		// M to be minimized without recording wavefunction
		Func<vector,vector> M = delegate(vector v){ 
		Func<double,vector,vector> diff = delegate(double r, vector f){vector w = new vector(2); w[0] = f[1];
                                                                w[1] = (-2.0/r - 2*v[0])*f[0]; return w;};
                vector res = ODE.driver(diff, rmin, fmin, rmax, acc: acc, eps: eps);
		vector tres = new vector(2); tres[0] = res[0] - rmax*Exp(-Sqrt(-2*v[0])*rmax); 
		tres[1] = res[1] - (1 - rmax*Sqrt(-2*v[0]))*Exp(-Sqrt(-2*v[0])*rmax); return tres;};

		//Start of convergence calculations
		int N = 10;
		vector rmaxs = new vector(N), Ermax = new vector(N), Estart = new vector("-1");
		//construct the values of the parameters
		for(int i=0;i<N;i++)rmaxs[i] = 6 + i;
		//rmax calculations
		for(int i=0;i<N;i++){rmax = rmaxs[i]; Ermax[i] = root.newton(M, Estart)[0];}
		//Write the data to a file
		for(int i=0;i<N;i++){
			WriteLine($"{rmaxs[i]} {Ermax[i]}");}
		return 0;
	}//Main
}//main
