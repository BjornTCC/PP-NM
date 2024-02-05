using System;
using static System.Console;
using static System.Math;

class main{
	static double sqrt2 = Sqrt(2.0);
	static double root52 = Pow(2.0, 0.2);
	static double epi = Pow(E, PI);
	static double pie = Pow(PI, E);
	static int Main(){
		Write("Part 1:\n");
		Write($"sqrt(2)={sqrt2}, sqrt(2)^2 = {sqrt2*sqrt2}\n");
		Write($"2^(1/5)={root52}, (2^(1/5))^5 = {root52*root52*root52*root52*root52}\n");
		Write($"e^pi={epi}, pi^e = {pie}\n");
		Write("Part 2:\n");
		double prod = 1;
		for(int x = 1; x<10; x+=1){
			Write($"fgamma({x})={sfuns.fgamma(x)}, {x-1}! ={prod}\n");
			prod *=x;
			}
		Write("Part 3:\n");
		for(int x = 1; x<10; x+=1){
                        Write($"lngamma({x})={sfuns.lngamma(x)}\n");
                        }
	return 0;
	}
}
