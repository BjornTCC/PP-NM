using System;
using static System.Math;

public class integrate{
	
	static double corput(int n, int b){
		double q=0, bk=(double)1.0/b;
		while(n>0){q+=(n%b)*bk; n/=b; bk/=b;}
		return q;
	}//corput
	 
	static void halton(int n, int d, vector x){
		int[] basis = new int[]{2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		if(d > basis.Length) throw new ArgumentException($"Halton: dimension too large: {d} > {basis.Length}.");
		for(int i = 0; i < d; i++) x[i] = corput(n, basis[i]);
	}//halton
	
	static void latrule(int n, int d, vector x){
		double[] primes = new double[]{2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		for(int i = 0; i < primes.Length; i++) primes[i] = Sqrt(primes[i]);
		if(d > primes.Length) throw new ArgumentException($"latrule: dimension too large: {d} > {primes.Length}.");
		for(int i = 0; i < d; i++) x[i] = (primes[i] *n) % 1;
	}//latrule

	public static (double,double) plainmc(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; for(int i=0;i<dim;i++)V*=b[i]-a[i];
        double sum=0,sum2=0;
	var x=new vector(dim);
	var rnd=new Random();
        for(int i=0;i<N;i++){
                for(int k=0;k<dim;k++)x[k]=a[k]+rnd.NextDouble()*(b[k]-a[k]);
                double fx=f(x); sum+=fx; sum2+=fx*fx;
                }
        double mean=sum/N, sigma=Sqrt(sum2/N-mean*mean);
        var result=(mean*V,sigma*V/Sqrt(N));
        return result;
	}//plainmc
	
	public static (double,double) quasimc(Func<vector,double> f,vector a,vector b,int N){
        int dim=a.size; double V=1; for(int i=0;i<dim;i++)V*=b[i]-a[i];
        double sum=0,sum2=0;
        var x=new vector(dim);
	vector halt = new vector(dim);
	vector lat = new vector(dim);
        for(int i=0;i<N;i++){
		halton(i, dim, halt);
		latrule(i, dim, lat);
                for(int k=0;k<dim;k++)x[k]=a[k]+halt[k]*(b[k]-a[k]);
                double fx=f(x); sum+=fx;
		for(int k=0;k<dim;k++)x[k]=a[k]+lat[k]*(b[k]-a[k]);
		fx=f(x); sum2+=fx;
                }
        double mean1 = sum/N, mean2=sum2/N, mean=(mean1+mean2)/2, sigma=Abs(mean1-mean2);
        var result=(mean*V,sigma*V);
        return result;
        }//plainmc
}//integrate
