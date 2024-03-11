using System;
using static System.Console;
using static System.Math;

public class main{
	public static int Main(){
		Func<vector,double> Rosen = v => Pow(1-v[0],2) + 100*Pow(v[1] - v[0]*v[0],2);
		Func<vector,double> Himmel = v => Pow(v[0]*v[0]+v[1]-11,2)+Pow(v[0]+v[1]*v[1]-7,2);
		int steps = 0;
		matrix vstart = new matrix("1 -1 -1 1; 1 1 -1 -1"), vmin = new matrix(2,4);
		vector fvmin = new vector(4);
		
		vstart.print("Starting vectors:");
		WriteLine("");

		WriteLine("Rosenbrocks function: f(x,y) = (1-x)^2 + 100*(y-x^2)^2");
		WriteLine("Minima: vmin = (1,1) f(vmin) = 0");
		WriteLine("");
		
                for(int i=0;i<4;i++){
                        (vmin[i],fvmin[i],steps) = min.qnewton(Rosen, vstart[i]);
			WriteLine($"Numerical minima: vmin = ({vmin[0,i]},{vmin[1,i]}), f(vmin) = {fvmin[i]}, steps = {steps}");
                }
		WriteLine("");

		WriteLine("Himmelblau's function: f(x,y) = (x^2+y-11)^2 + (x+y^2-7)^2");
		WriteLine("Minima: vmin = (3,2), f(vmin) = 0");
                WriteLine("Minima: vmin = (-2.805,3.313), f(vmin) = 0");
                WriteLine("Minima: vmin = (-3.779,-3.283), f(vmin) = 0");
                WriteLine("Minima: vmin = (3.584,-1.848), f(vmin) = 0");
		WriteLine("");

		for(int i=0;i<4;i++){
                        (vmin[i],fvmin[i],steps) = min.qnewton(Himmel, vstart[i]);
                        fvmin[i] = Himmel(vmin[i]);
                        WriteLine($"Numerical minima: vmin = ({vmin[0,i]},{vmin[1,i]}), f(vmin) = {fvmin[i]}, steps = {steps}");
		}
		return 0;
	}//Main
}//main
