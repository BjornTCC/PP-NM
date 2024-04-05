using System;
using static System.Console;
using static System.Math;

public static class main{
	public static int Main(string[] args){
		//Read the data from file
		string infile = null;
		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0]=="-input")infile=words[1];
		}
		if(infile==null){Error.WriteLine("Wrong filename"); return 1;}
		genlist<double> _t = new genlist<double>(), _y = new genlist<double>(), _dy = new genlist<double>();
		var instream = new System.IO.StreamReader(infile);
		for(string line = instream.ReadLine();line!=null;line=instream.ReadLine()){
			var nums = line.Split(' ');
			_t.add(double.Parse(nums[0]));
                        _y.add(double.Parse(nums[1]));
                        _dy.add(double.Parse(nums[2]));
		}
		vector t = _t.get_data();
		vector y = _y.get_data();
		vector dy = _dy.get_data();

		//begin processing
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
