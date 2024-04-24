using System;
using static System.Console;
using static System.Math;

public class main{
	
	public class BBPSO{
		
		public int N, dim, steps, f_eval;
		public double f;
		public vector x;

		//constructor
		public BBPSO(
			Func<vector,double> F, 		/* Function to be minimized */ 
			vector a, 			/* corners of square region */
			vector b,
			int num,			/* number of particles */
			int seconds = 10		/* max allowed steps, change to alloted time later */ 
			){
			//initalize data
			dim = a.size;
			N = num;
			steps = 0; f_eval = 0;
			vector[] xs = new vector[N];
			vector[] p = new vector[N];
			vector fs = new vector(N);
			vector gmin = new vector(dim);

			// initialize vectors
			for(int i=0;i<N;i++){
				xs[i] = vuniform(a,b); 
				p[i] = xs[i];
				fs[i] = F(xs[i]);}
			f_eval += N;
			f = fs[0]; gmin = xs[0];
			for(int i=1;i<N;i++){
				if(fs[i] < f){f = fs[i]; gmin = xs[i];}
			}
			// Do evolution
			var start_time = DateTime.Now;
			do{
				steps++;
				for(int i=0;i<N;i++){
					xs[i] = vnormal(0.5*(p[i]+gmin), (p[i]-gmin).norm(),a,b);
					double f_temp = F(xs[i]);
					if(f_temp < f){f = f_temp; gmin = xs[i]; p[i] = xs[i];}
					else if(f_temp < fs[i]){fs[i] = f_temp; p[i] = xs[i];}
				}
			}while((DateTime.Now-start_time).Seconds < seconds);
			x = gmin;
		}

		static double normal(double m, double σ){
			Random unif = new Random();
			double res = 0;
			for(int i=0;i<12;i++)res+=unif.NextDouble();
			res-=6;
			res*=σ;
			res+=m;
			return res;
		}//normal

		static vector vnormal(vector m, double σ,vector a, vector b){
			vector res = new vector(m.size);
			for(int i=0;i<res.size;i++)res[i] = Min(Max(normal(m[i],σ),a[i]),b[i]);
			return res;
		}//vnormal

		static vector vuniform(vector a, vector b){
                        Random uniform = new Random();
			vector res = new vector(a.size);
			for(int i=0;i<res.size;i++)res[i] = a[i] + uniform.NextDouble()*(b[i]-a[i]);
			return res;
		}//vuniform
	}//BBPSO

	public static int Main(){
		Func<vector, double> f = x => 0.05*x.dot(x)-Cos(x.norm());
		vector a = new vector("-10, -10, -10");
		vector b = new vector("10, 10, 10");
		int N = 10;
		BBPSO min = new BBPSO(f,a,b,N);
		WriteLine($"min f: {min.f}");
		min.x.print("min x: ");
		WriteLine($"Steps: {min.steps}");	
		return 0;
	}//Main
}//main
