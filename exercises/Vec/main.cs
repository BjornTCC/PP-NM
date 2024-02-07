using System;
using static System.Console;
using static System.Math;

static class main{
	static void Main(){
		vec V1 = new vec();
		vec V2 = new vec(1.0,1.0,1.0);
		vec V3 = new vec(2,3,4);
		V1.print("V1 = ");
		V1.print();
		V2.print("V2 = ");
		V2.print();
                V3.print("V3 = ");
                V3.print();
		WriteLine($"2*V3 = {2*V3} = V3*2 = {V3*2}");
	}
}
