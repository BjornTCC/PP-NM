using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<double[],double,double> phi = delegate(double[] y,double x){
								return y[2]+y[0];};
		ann.diff_eq Network = new ann.diff_eq(10);
		double[] y0 = {1,0}; double xl = PI, xm = -PI/4;
		Network.train(phi,xm,xl,0,y0,10,10,acc:1e-7);
		int M = 100; double z = xm;
		for(int i=0;i<M;i++){WriteLine($"{z} {Network.response(z)}");z+=(xl-xm)/(M-1);}
		return 0;
	}//Main
}//main
