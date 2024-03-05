using System;
using static System.Console;
using static System.Math;

public class main{
	public static void Main(){
		WriteLine("Code for solving tri-diag. systems can be found in PP-NM/libs/QRDecomp/QRGS.cs.");
		WriteLine("I wanted to place it next to the other lin-eq. solvers.");
		int N = 5;
		matrix A = new matrix(N); //Generate som random tridiag. matrix
		vector b = new vector(N);
		Random random = new Random();
		for(int i = 0; i < N; i++){
			A[i,i] = random.NextDouble();
			b[i] = random.NextDouble();
			if(i!=N-1){
				A[i, i+1] = random.NextDouble();
				A[i+1, i] = random.NextDouble();}}
		matrix Ac = A.copy();
		vector bc = b.copy();
		vector TriSol = linsol.TriDiagSol(Ac, bc);
		vector compSol = QRGS.solve(A, b);
		vector ATriSol = A*TriSol;
		vector AcompSol = A*compSol;
		if(TriSol.approx(compSol))WriteLine("Test of TriDiagSol succesful");
		else WriteLine("Test of TriDiagSol failed");
		A.print("A:");
		b.print("b:");
		TriSol.print("x:");
		ATriSol.print("A*x:");
		compSol.print("known solution:");
	}//Main
}//main
