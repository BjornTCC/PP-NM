using System;
using static System.Console;
using static cmath;

static class main{
	static void Main(){
		WriteLine($"sqrt(-1) = {cmath.sqrt(new complex(-1))} = i? {cmath.sqrt(new complex(-1)).approx(I)}");
		WriteLine($"sqrt(i) = {cmath.sqrt(I)} = sqrt(0.5)*(1+i)? {cmath.sqrt(I).approx(new complex(System.Math.Sqrt(0.5),System.Math.Sqrt(0.5)))}");
		WriteLine($"e^i = {cmath.exp(I)}");
		WriteLine($"e^pi*i = {cmath.exp(System.Math.PI*I)} = -1? {cmath.approx(-1,cmath.exp(System.Math.PI*I))}");
		WriteLine($"i^i = {cmath.pow(I,I)} = e^-pi/2? {cmath.approx(cmath.exp(-System.Math.PI/2), cmath.pow(I,I))}");
		WriteLine($"ln(i) = {cmath.log(I)} = i*pi/2? {cmath.log(I).approx(I*System.Math.PI/2)}");
		WriteLine($"sin(i*pi) = {cmath.sin(System.Math.PI*I)} = i*sinh(pi)? {cmath.sin(System.Math.PI*I).approx(I*cmath.sinh(System.Math.PI))}");
	}
}
