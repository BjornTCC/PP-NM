using System;
using static System.Console;
using static System.Math;

public class main{
	
	public class gen_evd{
		public matrix V, L, LT, L_i, LT_i;
		public vector E;
		public readonly int dim;

		//constructor
		public gen_evd(matrix A, matrix B){
			dim = A.size1;
			cholesky(B);
			invL();
			A = L_i*A*LT_i;
			(E,V) = jacobi.cyclic_opt(A);
			V = LT_i*V;	
		}//constructor
		
		public void cholesky(matrix B){
			L = new matrix(dim); 
			for(int i=0;i<dim;i++)
			for(int j=0;j<=i;j++){
				double sum = 0;
				for(int k=0;k<j;k++)sum+=L[i,k]*L[j,k];
				if(i==j) L[i,j] = Pow(B[i,j]-sum,0.5);
				else L[i,j] = (B[i,j]-sum)/L[j,j];
			}
			LT = L.transpose();
		}//cholesky
		
		public void invL(){
			L_i = new matrix(dim);
			for(int i = 0; i < dim; i++){
				L_i[i,i] = 1.0/L[i,i];
				for(int j = i+1; j < dim; j++){
					double sum = 0;
					for(int k = j; k >= 0; k--)sum+= L[j,k]*L_i[k,i];
					L_i[j,i] = -sum/L[j,j];
				}
			}
			LT_i = L_i.transpose();
		}//invL

	}//gen_evd

	public static int Main(){
		int n = 6;
		matrix A = new matrix(n), vs = new matrix(n), B = new matrix(n);
		System.Random random = new System.Random();
		for(int i = 0; i < n; i++){
			for(int j = 0; j < n; j++) vs[i][j] = 2*random.NextDouble()-1;}	
		for(int i = 0; i < n; i++)
		for(int j = i; j < n; j++){
			B[i,j] = vs[i].dot(vs[j]);
			B[j,i] = B[i,j];
			A[i,j] = 2*random.NextDouble() - 1;
			A[j,i] = A[i,j];
		}

		gen_evd EVD = new gen_evd(A.copy(),B.copy());
		
		B.print("B = ");
		EVD.L.print("L = ");
		matrix LLT = EVD.L*EVD.LT;
		LLT.print("L*LT = ");
		matrix LL_i = EVD.L*EVD.L_i;
		LL_i.print("L*L^-1 = ");

                matrix L_iL = EVD.L_i*EVD.L;
                L_iL.print("L^-1*L = ");

		WriteLine("---------------------------------------------------------------");
		EVD.E.print("E = ");
		EVD.V.print("V = ");

		matrix AV = A*EVD.V;
		AV.print("A*V = ");

		matrix ES = matrix.diag(EVD.E);

		matrix EBV = B*EVD.V*ES;
		EBV.print("B*V*E = ");
		
		return 0;
	}//Main

}//main
