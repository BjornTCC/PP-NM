using System;
using static System.Console;
using static cmath;

public static class main{
	public static void Main(){
		for(double x = -4; x <= 4; x+=1.0/8){
			for(double y = -5; y<= 5; y+=1.0/8){
				complex z = new complex(x,y);
				complex gam = sfuns.cgamma(z);
				WriteLine($"{x} {y} {abs(gam)} {arg(gam)}");
			}
		}
	}//Main
}//main
