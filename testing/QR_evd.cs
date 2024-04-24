using System;
using static System.Console;
using static System.Math;

public class main{
	
	public class QR_evd{
	
		public matrix D, V;
		public vector w;
		public int dim,steps;

		//constructor
		public QR_evd(matrix A, double shift = 0){
			// Initialize all data for loop
			dim = A.size1;
			int n = dim-1;
			A = A - shift*matrix.id(dim);
			V = matrix.id(dim);
			matrix Q,R,A_copy = A.copy();
                        D = A.copy();
			w = new vector(dim);w[dim-1] = A[dim-1,dim-1];
			do{
				steps++;			
				(Q,R) = QRGS.decomp(A_copy);			/* Do decomposition of small matrix */
				V = Q_mult(V,Q);				/* V <- V*Q */
				A_copy = R*Q;					/* A <- R*Q */
				D = RQ_big(D,A_copy);				/* A <- R*Q but for the large diagonal matrix */
				if(A_copy[n,n] != w[n]){w[n] = A_copy[n,n];}	/* check for convergence of eigenvalue */
				else{
					if(n == 1) break;			/* leave if 2nd to last eigenvalue is found */
					n--;
					A_copy = new matrix(n+1);		/* delete column and row in A */
					for(int i=0;i<n+1;i++)for(int j=0;j<n+1;j++)A_copy[i,j] = D[i,j];	
					w[n] = A_copy[n,n];			
				}
                                //Error.WriteLine($"Made it here! n: {n}, steps: {steps}. Δ = {w[n]-D[n,n]}");
			}while(true);
                        for(int i=0;i<dim;i++)w[i] += shift;
			D+= shift*matrix.id(dim);
			w[0] = D[0,0];						/* 0'th element of w is never updated in loop */
		}

		public static matrix Q_mult(matrix A, matrix Q){
			int dimA = A.size1, dimQ = Q.size1;
			matrix C = A.copy();
			for(int i=0;i<dimA;i++)
			for(int j=0;j<dimQ;j++){
				C[i,j]=0;
				for(int k=0;k<dimQ;k++)C[i,j] += A[i,k]*Q[k,j];
			}

			return C;
		}
		public static matrix RQ_big(matrix D, matrix A){
                        for(int i=0;i<A.size1;i++)for(int j=0;j<A.size2;j++)D[i,j] = A[i,j];
			return D;
                }
	}//QR_evd
	
	public static int Main(){
		int N = 3;
		matrix A = new matrix(N);
		System.Random random = new System.Random();
		for(int i = 0; i < N; i++)
		for(int j = i; j < N; j++){
			A[i,j] = random.NextDouble();
			A[j,i] = A[i,j];
		}
		A.print("A = ");
		int d = 1;
                matrix A_big = matrix.id(N+d),C = new matrix(N+d);;
                for(int i = 0; i < N+d; i++)
                for(int j = 0; j < N+d; j++){
                        C[i,j] = random.NextDouble();
                        if(i<N && j<N)A_big[i,j] = A[i,j];
                }

                A_big.print("A = ");
                C.print("C = ");
                matrix CA = C*A_big;
                matrix CA_test = QR_evd.Q_mult(C,A);
                CA.print("C*A = ");
                CA_test.print("Q_mult(C,A) = ");
                if(CA.approx(CA_test)) WriteLine("Test succes");
                else WriteLine("Test failure");
		WriteLine("-------------------------------------------------------------------------");
		A.print("A = ");
		matrix A_new = A.copy();
		var EVD = new QR_evd(A_new);
		matrix V = EVD.V, D = EVD.D;
		vector w = EVD.w;
		int steps = EVD.steps;
		matrix A_test = V*D*V.transpose();
		matrix id_test1 = V*V.transpose();
		matrix id_test2 = V.transpose()*V;
		WriteLine($"Steps: {steps}");
		A_test.print("V*D*V^T = ");
		w.print("w = ");
		D.print("D = ");
		V.print("V = ");
		id_test1.print("V*V^T = ");
		id_test2.print("V^T*V = ");
		matrix id = matrix.id(N);
		if(A.approx(A_test) && id_test1.approx(id) && id_test2.approx(id)) WriteLine("Test succes");
		else WriteLine("Test failure");
		return 0;
	}//Main
}//main
