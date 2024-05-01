using System;
using static System.Console;
using static System.Math;

public class main{
	
	public static class FT{
		
		public static complex[] DFT(complex[] x){
			int N = x.Length;
			complex[] c = new complex[N];
			for(int k=0;k<N;k++){
				c[k] = complex.Zero;
				complex ω = 2*PI*complex.I*k/N;
				for(int n=0;n<N;n++)c[k]+=x[n]*cmath.exp(-ω*n);
			}
			return c;
		}//DFT
		
		public static complex[] inv_DFT(complex[] x){
			int N = x.Length;
                        complex[] c = new complex[N];
                        for(int k=0;k<N;k++){
                                c[k] = complex.Zero;
                                complex ω = 2*PI*complex.I*k/N;
                                for(int n=0;n<N;n++)c[k]+=x[n]*cmath.exp(ω*n);
                                c[k]/=N;
                        }
                        return c;
		}//inv_DFT
		
		public static complex[] FFT(complex[] x){
			int N = x.Length;
			complex[] c = new complex[N];
			
			return c;
		}
	}//FT

	public static int Main(){
		int p = 3; int N = (int)Pow(2,p);
		complex[] x = new complex[N];
		Random random = new Random();
		for(int i=0;i<N;i++){
			double re = 2*random.NextDouble() - 1, im = 2*random.NextDouble() - 1;
			x[i] = new complex(re,im);
		}
		WriteLine("x = ");
		foreach(complex y in x)WriteLine(y);
		complex[] c = FT.DFT(x);
		complex[] x_inv = FT.inv_DFT(c);
		
                WriteLine("c = ");
                foreach(complex y in c)WriteLine(y);


                WriteLine("x_inv = ");
                foreach(complex y in x_inv)WriteLine(y);
		bool test = true;
		for(int i=0;i<N;i++){
			if(!x[i].approx(x_inv[i])) test = false;
		}
		if(test) WriteLine("Simple + inverse: Test success");
		else WriteLine("Simple + inverse: Test failure");
		return 0;
	}//Main
}//main
