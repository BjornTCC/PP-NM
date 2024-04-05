using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<Func<double,double>[],double,double> phi = delegate(Func<double,double>[] y,double x){
								return y[2](x)+y[1](x)+y[0](x);};
		diff_nn Network = new diff_nn(10);
		double[] y0 = {1,0}; double xl = PI, xm = -PI/4;
		Network.train(phi,xm,xl,0,y0,2,2);
		int M = 100; double z = xm;
		for(int i=0;i<M;i++){WriteLine($"{z} {Network.response(z)}");z+=(xl-xm)/(M-1);}
		return 0;
	}//Main
}//main
