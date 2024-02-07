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
		WriteLine(V1.ToString());
		V2.print("V2 = ");
		V2.print();
                WriteLine(V2.ToString());
                V3.print("V3 = ");
                V3.print();
                WriteLine(V3.ToString());
		WriteLine($"2*V3 = {2*V3} = V3*2 = {V3*2}");
                WriteLine($"-V3 = {-V3}");
                WriteLine($"V2+V3 = {V2+V3} = V3+V2 = {V3+V2}");
                WriteLine($"V2-V3 = {V2-V3}, V3-V2 = {V3-V2}");
		WriteLine($"V1.V2 = {V1.dot(V2)} = {vec.dot(V1,V2)}");
                WriteLine($"V2.V3 = {V2.dot(V3)} = {vec.dot(V2,V3)}");
		WriteLine($"V2 = V3: {V2.approx(V3)}");
		WriteLine($"V3 = V3: {V3.approx(V3)}");
		WriteLine($"V3 = V3 + epsilon: {V3.approx(V3*(1+1e-11))}");
	}
}

