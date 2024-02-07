using System;
using static System.Console;
using static System.Math;

class main{
	static int i = 1;
	static double x = 1;
	static float y = 1F;
	static int n = (int)1e6;
	static double epsilon = Pow(2,-52);
	static double tiny = epsilon/2;
	static double sumA=0, sumB=0;
	static double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
	static double d2 = 8*0.1;
	public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9){
		if(Abs(b-a) <= acc) return true;
		if(Abs(b-a) <= Max(Abs(a),Abs(b))*eps) return true;
		return false;
	}
	static int Main(){
		Write("Part 1:\n");
		while (i+1 > i) {i++;}
		Write($"my max int = {i}, tabulated = {int.MaxValue}\n");
		i = 0;
		while (i-1 < i) {i--;}
		Write($"my min int = {i}, tabulated = {int.MinValue}\n");
		
		Write("Part 2:\n");
	       	while(1+x!=1){x/=2;} 
		x*=2;
		while((float)(1F+y) != 1F){y/=2F;} 
		y*=2F;
		Write($"double machine e = {x}, 2^-52 = {Pow(2, -52)}\n");
		Write($"float machine e = {y}, 2^-23 = {Pow(2, -23)}\n");
		
		Write("Part 3:\n");
		sumA+=1; for(int i=0;i<n;i++){sumA+=tiny;}
		for(int i=0;i<n;i++){sumB+=tiny;} sumB+=1;
		WriteLine($"sumA-1 = {sumA-1:e} should be {n*tiny:e}");
		WriteLine($"sumB-1 = {sumB-1:e} should be {n*tiny:e}");
		
		Write("Part 4:\n");
		WriteLine($"d1={d1:e15}");
		WriteLine($"d2={d2:e15}");
		WriteLine($"d1==d2 ? => {d1==d2}");

		Write("Comparing doubles: task\n");
                WriteLine($"d1={d1:e15}");
                WriteLine($"d2={d2:e15}");
                WriteLine($"d1 ~ d2 ? => {approx(d1,d2, 2*epsilon)}");
	return 0;
	}
}
