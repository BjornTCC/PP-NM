using System;
using static System.Console;
using static System.Math;

public class main{

	public class glo_sto{
	
	public readonly int dim, f_eval;
	public readonly vector x;
	public readonly double f;

	//constructor
	public glo_sto(Func<vector,double> φ, vector a, vector b, int seconds,double acc =1e-10){
		dim = a.size;
		f_eval = 0;
		f = Double.PositiveInfinity;
		x = new vector(dim);
		vector x_temp = new vector(dim);
		var start_time = DateTime.Now;
		do{
			f_eval++;
			halton(f_eval, dim, x_temp);
			for(int i=0;i<dim;i++) x_temp[i] = a[i] + x_temp[i]*(b[i]-a[i]);
			double f_temp = φ(x);
			if(f_temp < f){f = f_temp; x = x_temp;}
		}while((DateTime.Now-start_time).Seconds < seconds);
		min.downhill_sim mini = new min.downhill_sim(φ,x,acc);
		x = mini.x;
		f = mini.f;
		f_eval += mini.f_eval;
	}

	static double corput(int n, int b){
		double q=0, bk=(double)1.0/b;
		while(n>0){q+=(n%b)*bk; n/=b; bk/=b;}
		return q;
	}//corput
	 
	static void halton(int n, int d, vector x){
		int[] basis = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		if(d > basis.Length) throw new ArgumentException($"Halton: dimension too large: {d} > {basis.Length}.");
		for(int i = 0; i < d; i++) x[i] = corput(n, basis[i]);
	}//halton
	
	}//glo_sto

	public static int Main(){
		Func<vector, double> f = x => 0.05*x.dot(x)+Cos(x.norm());
		vector a = new vector("-10, -10, -10");
		vector b = new vector("10, 10, 10");
		glo_sto min = new glo_sto(f,a,b,5);
		WriteLine($"min f: {min.f}");
		min.x.print("min x: ");
		WriteLine($"f_eval: {min.f_eval}");
		return 0;
	}//Main
}//main
