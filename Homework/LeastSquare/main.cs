using System;
using static System.Console;
using static System.Math;

public static class main{
	public static int Main(){
		vector t = new vector("1 2 3 4 6 9 10 13 15");
		vector y = new vector("117 100 88 72 53 29.5 25.2 15.2 11.1");
		vector dy = new vector("6 5 4 4 4 3 3 2 2");
		vector lny = new vector(9), dlny = new vector(9);
		for(int i = 0; i < 9; i++){
			lny[i] = Log(y[i]);
			dlny[i] = dy[i]/y[i];}
		Func<double,double>[] fs = new Func<double,double>[]{x => 1, x => -x};
		(vector c, matrix cov) = fit.lsfit(fs, t, lny, dlny);
		c.print("c:");
		WriteLine($"Uncertainties: {Sqrt(cov[0,0])} {Sqrt(cov[1,1])}");
		double tau = Log(2)/c[1];
		double dtau  = Log(2)/(c[1]*c[1])*Sqrt(cov[1,1]);
		WriteLine($"a = {Exp(c[0])}");
		WriteLine($"tau = {tau} pm {dtau} days, table value: 3.6313(14) days");
		cov.print("covariance matrix:");
		return 0;
	}//Main
}//main
