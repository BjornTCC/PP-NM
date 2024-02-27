using System;
using static System.Console;
using static System.Math;

static class main{
	static void Main(){
		matrix M = new matrix("1 0 0 0; 0 1 0 0; 0 0 1 0"); 
		M.print();
		M.set(1,2,2.0);
		M.print();
		double[] a = new double[]{10,20,30};
		double[] b = new double[]{-1, -2, -3, -4};
		M.set(0, a);
		M.print();
		M.set(1,b, row: true);
		M.print();
	}
}

