using System;
using static System.Console;
using static System.Math;

public static class main{
	public static int Main(){
		int N = 40; /* number of sample points */ 
		int[] ns = new int[3]{3,5,9};
		Func<double,double> g = X => Cos(5*X-1)*Exp(-X*X);
		vector x = new vector(N), y = new vector(N);
		for(int i=0;i<N;i++){
			x[i] = -1+2.0*i/(N-1); y[i] = g(x[i]);
			Error.WriteLine($"{x[i]} {y[i]}");}
		ann[] Networks = new ann[ns.Length];
		for(int i=0;i<3;i++){
			Networks[i] = new ann(ns[i]);
			Networks[i].train_interp(x,y);
		}
		int M = 100;
		double z = -1;
		for(int i=0;i<M;i++){
			Out.WriteLine($"{z} {Networks[0].response(z)} {Networks[0].dresponse(z)} {Networks[0].Iresponse(-1,z)} {Networks[1].response(z)} {Networks[1].dresponse(z)} {Networks[1].Iresponse(-1,z)} {Networks[2].response(z)} {Networks[2].dresponse(z)} {Networks[2].Iresponse(-1,z)}");
			z+=2.0/(M-1);
		}
		return 0;
	}//Main
}//main
