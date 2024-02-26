using System;
using static System.Console;
using static System.Math;

static class main{
	static void Main(){
		matrix M = new matrix("1 0 1; 0 1 0 ; 0 0 1");
		matrix b = matrix.id(3);
		b.print();
		matrix B = M*b; 
		M.print();
		B.print();
		WriteLine(matrix.norm(M[0]));
	}
}

