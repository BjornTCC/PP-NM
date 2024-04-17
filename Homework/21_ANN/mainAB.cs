using System;
using static System.Console;
using static System.Math;

public static class main{
	public static int Main(string[] args){
		//Config extra output file
		string outfile=null;
		foreach(var arg in args){
			string[] words = arg.Split(':');
			if(words[0]=="-output")outfile=words[1];
		}
		if(outfile==null){
			System.Console.Error.WriteLine("wrong argument");
			return 1;
		}
		var outstream = new System.IO.StreamWriter(outfile,append:false);
		
		//start of actual program
		int N = 40; /* number of sample points */ 
		int[] ns = new int[3]{3,5,9};
		Func<double,double> g = X => Cos(5*X-1)*Exp(-X*X);
		vector x = new vector(N), y = new vector(N);
		for(int i=0;i<N;i++){
			x[i] = -1+2.0*i/(N-1); y[i] = g(x[i]);
			outstream.WriteLine($"{x[i]} {y[i]}");}
		ann.interpol[] Networks = new ann.interpol[ns.Length];
		for(int i=0;i<3;i++){
			Networks[i] = new ann.interpol(ns[i]);
			Networks[i].train_interp(x,y);
			Error.WriteLine($"Training with {ns[i]} nodes terminated with success: {Networks[i].train_status}");
		}
		int M = 100;
		double z = -1;
		for(int i=0;i<M;i++){
			Out.WriteLine($"{z} {Networks[0].response(z)} {Networks[0].dresponse(z)} {Networks[0].Iresponse(-1,z)} {Networks[1].response(z)} {Networks[1].dresponse(z)} {Networks[1].Iresponse(-1,z)} {Networks[2].response(z)} {Networks[2].dresponse(z)} {Networks[2].Iresponse(-1,z)}");
			z+=2.0/(M-1);
		}
		outstream.Close();
		return 0;
	}//Main
}//main
