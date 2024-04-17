using System;
using static System.Console;
using static System.Math;

public static class main{

	public class cholesky{
		public readonly matrix A;
		public readonly matrix L;
		public readonly matrix LT;
		public readonly int dim;

		//constructor
		public cholesky(matrix B){
			if(B.size1 != B.size2) throw new ArgumentException($"LLT: non-square matrix, size: ({B.size1}, {B.size2})");
			dim = B.size1;
			L = new matrix(dim); A = B;
			for(int i=0;i<dim;i++)
			for(int j=0;j<=i;j++){
				double sum = 0;
				for(int k=0;k<j;k++)sum+=L[i,k]*L[j,k];
				if(i==j) L[i,j] = Pow(B[i,j]-sum,0.5);
				else L[i,j] = (B[i,j]-sum)/L[j,j];
			}
			LT = L.transpose();
		}

		//methods

		public double determinant(){
			double res = 1;
			for(int i=0;i<dim;i++)res*=L[i,i];
			return res*res;
		}

		vector backsub(vector b){ //back substitution inplace
			for(int i = dim-1; i >= 0; i--){
	                        double sum = 0;
        	                for(int j = i + 1; j < dim; j++)sum+=LT[i,j]*b[j];
	                        b[i] = (b[i] - sum)/LT[i,i];
                		}
                	return b;
		}//backsub

		vector forwardsub(vector b){ //forward substitution inplace
                        for(int i = 0; i < dim; i++){
                                double sum = 0;
                                for(int k = 0; k < i; k++)sum+=L[i,k]*b[k];
                                b[i] = (b[i] - sum)/L[i,i];
                                }
                        return b;
                }//backsub
		
		public vector linsol(vector b){ //solves the problem inplace
			return backsub(forwardsub(b));
		}

		public matrix inverse(){
			matrix Ainv = matrix.id(dim);
			for(int i=0;i<dim;i++)linsol(Ainv[i]);
			return Ainv;
		}

	}//cholesky

	public static int Main(){
		int n = 5;
		//Create a random positive definite matrix using A_ij = V_i.V_j
		matrix A = new matrix(n); matrix vs = new matrix(n);
		System.Random random = new System.Random();
		for(int i = 0; i < n; i++){
			for(int j = 0; j < n; j++) vs[i][j] = random.NextDouble();}	
		for(int i = 0; i < n; i++)
		for(int j = i; j < n; j++){
			A[i,j] = vs[i].dot(vs[j]);
			A[j,i] = A[i,j];
		}
		//test decomposition
		A.print("A =");
		cholesky LLT = new cholesky(A);
		LLT.L.print("L=");
		matrix res = LLT.L*LLT.LT;
		res.print("LLT=");
		if(res.approx(A)) WriteLine("LLT = A: Test success");
		else WriteLine("LLT = A: Test Failure");
		WriteLine("--------------------------------------------------------------");
		//test determinant
		double det = LLT.determinant();
		WriteLine($"LLT det: {det}");
		double QR_det = Abs(QRGS.det(A));
                WriteLine($"QR det: {QR_det}");
		if(Abs(det-QR_det)<1e-6) WriteLine("Determinant test success");
		else WriteLine("Determinant test failure");
		WriteLine("--------------------------------------------------------------");
		//test linear equation solver
		vector y = new vector(n);
		for(int i=0;i<n;i++)y[i] = random.NextDouble();
		vector x = y.copy();
		y.print("y=");
		LLT.linsol(x);
		x.print("x=");
		vector Ax = A*x;
		Ax.print("A*x=");
		if(y.approx(Ax)) WriteLine("Equation solver test: Success");
		else WriteLine("Equation solver test: failure");
		WriteLine("-------------------------------------------------------------");
		//test inverse matrix
		matrix Ainv = LLT.inverse();
		Ainv.print("A^-1=");
		matrix AAinv = A*Ainv;
                matrix AinvA = Ainv*A;
		AAinv.print("A*A^-1=");
                AinvA.print("A^-1*A=");
		if(AAinv.approx(AinvA) && AAinv.approx(matrix.id(n))) WriteLine("inverse test success");
		else WriteLine("Inverse test failure");
		return 0;
	}//Main
}//main
