using System;
using static System.Console;
using static System.Math;

public static class main{
	
	public class LU{
	
	public readonly int dim;
	public readonly matrix L, U, A;
	
	//constructor
	
	public LU(matrix B){
		if(B.size1!=B.size2) throw new ArgumentException($"LU: non-square matrix A: ({B.size1},{B.size2})");
		A = B.copy();
		dim = A.size1;
		L = matrix.id(dim); U = new matrix(dim);
		for(int i=0;i<dim;i++){
			for(int j=i;j<dim;j++){
				double sum = 0;
				for(int k=0;k<j;k++)sum+=L[i,k]*U[k,j];
				U[i,j]=A[i,j] - sum;
			}	
			for(int j = i+1;j<dim;j++){
                                double sum = 0;
                                for(int k=0;k<j;k++)sum+=L[j,k]*U[k,i];
				L[j,i] = (A[j,i]-sum)/U[i,i];
			}	
		}
	}
	
	public double determinant(){
                double res = 1;
                for(int i=0;i<dim;i++)res*=U[i,i];
                return res;
                }

	vector backsub(vector b){ //back substitution inplace
        	for(int i = dim-1; i >= 0; i--){
                	double sum = 0;
                        for(int j = i + 1; j < dim; j++)sum+=U[i,j]*b[j];
                        	b[i] = (b[i] - sum)/U[i,i];
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
	
	}//LU

	public static int Main(){
		int n = 5;
                //Create a random positive definite matrix using A_ij = V_i.V_j
                matrix A = new matrix(n);
                System.Random random = new System.Random();
                for(int i = 0; i < n; i++)
                for(int j = 0; j < n; j++){
                        A[i,j] = random.NextDouble();
                }
                //test decomposition
                A.print("A =");
                LU A_LU = new LU(A);
                A_LU.L.print("L=");
                A_LU.U.print("U=");
                matrix res = A_LU.L*A_LU.U;
                res.print("LU=");
                if(res.approx(A)) WriteLine("LLT = A: Test success");
                else WriteLine("LU = A: Test Failure");
                WriteLine("--------------------------------------------------------------");
		//test determinant
                double det = A_LU.determinant();
                WriteLine($"LU det: {det}");
                double QR_det = QRGS.det(A);
                WriteLine($"QR det: {QR_det}");
                if(Abs(Abs(det)-Abs(QR_det))<1e-6) WriteLine("Determinant test success");
                else WriteLine("Determinant test failure");
                WriteLine("--------------------------------------------------------------");
                //test linear equation solver
                vector y = new vector(n);
                for(int i=0;i<n;i++)y[i] = random.NextDouble();
                vector x = y.copy();
                y.print("y=");
                A_LU.linsol(x);
                x.print("x=");
                vector Ax = A*x;
                Ax.print("A*x=");
                if(y.approx(Ax)) WriteLine("Equation solver test: Success");
                else WriteLine("Equation solver test: failure");
                WriteLine("-------------------------------------------------------------");
                //test inverse matrix
                matrix Ainv = A_LU.inverse();
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
