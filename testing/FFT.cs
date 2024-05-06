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
			FFT_rec(c,x,N,1,0,0);
			return c;
		}//FFT

		public static void FFT_rec(complex[] c, complex[] x, int N, int step, int ic, int ix){
			/* Fourier transform implemented recursively of x[ix + n*step] for n = 0,1, ... N.
			 * Stored in c[ic + k] */ 
			if(N==1) c[ic] = x[ix]; 			/* Fourier transform of a single point */
			else if(N%2==0){
				FFT_rec(c,x,N/2,2*step,ic,ix);		/* Store even transform in c[0-N/2] */
                                FFT_rec(c,x,N/2,2*step,ic+N/2,ix+step);	/* Store odd transform in c[N/2-N] */
				for(int k=0;k<N/2;k++){
					complex p = c[ic+k], q = cmath.exp(-2*PI*complex.I*k/N)*c[ic+N/2+k];
					c[ic+k] = p+q;
					c[ic+k+N/2] = p-q;
				}
			}
			else dft(c,x,N,step,ic,ix);
		}//FFT_rec

		public static void dft(complex[] c, complex[] x, int N, int step, int ic, int ix){
			for(int k=0;k<N;k++){
                                c[ic+k] = complex.Zero;
                                complex ω = 2*PI*complex.I*k/N;
                                for(int n=0;n<N;n++)c[ic+k]+=x[ix+n*step]*cmath.exp(-ω*n);
                        }
		}//dft
	}//FT

	public static int Main(){
		int p = 3; int N = (int)Pow(2,p)+4;
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
		WriteLine("----------------------------------------------------------------------");
		complex[] c_fft = FT.FFT(x);
		WriteLine("c_fft = ");
                foreach(complex y in c_fft)WriteLine(y);
		
                test = true;
                for(int i=0;i<N;i++){
                        if(!c[i].approx(c_fft[i])) test = false;
                }
                if(test) WriteLine("FFT: Test success");
                else WriteLine("FFT: Test failure");
		return 0;
	}//Main
}//main
