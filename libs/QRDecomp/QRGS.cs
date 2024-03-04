using static System.Math;

public static class QRGS{
	
	public static (matrix, matrix) decomp(matrix A){ //returns in format [Q, R]
		int m = A.size2;
		matrix Q=A.copy(), R=new matrix(m,m);
		for(int i = 0; i<m; i++){
			R[i,i]=matrix.norm(Q[i]);
			Q[i]/=R[i,i]; //normalize the Q vectors
			for(int j=i+1; j<m; j++){
				R[i,j]=Q[i].dot(Q[j]);
				Q[j] -= Q[i]*R[i,j]; //Orthognalize the Q vectors
			}}
		return (Q, R);
	}//decomp	
	
	public static vector backsub(matrix A, vector b){ //back substitution inplace
		for(int i = b.size-1; i >= 0; i--){
                        double sum = 0;
                        for(int j = i + 1; j < b.size; j++)sum+=A[i,j]*b[j];
                        b[i] = (b[i] - sum)/A[i,i];
                	}
                return b;
	}//backsub

	public static vector solve(matrix A, vector b){ 
		(matrix Q, matrix R) = decomp(A);
		vector sol = Q.transpose()*b;
		return backsub(R, sol);
	}//solve
	
	public static double det(matrix A){ //only returns determinant up to sign
		if(A.size1 == A.size2){
			(matrix Q, matrix R) = decomp(A);
			double res = 1;
			for(int i = 0; i < R.size1; i++)res*=R[i,i];
			return res;
		}
		throw new System.ArgumentException($"Can't take determinant of non-square matrix with size ({A.size1}, {A.size2}).");
	}//det
	 
	public static matrix inv(matrix A){
		if(A.size1 == A.size2){
			int n = A.size1;
			matrix Ainv = new matrix(n);
			(matrix QT, matrix R) = decomp(A);
			QT = QT.transpose();
			for(int i = 0; i < n; i++)Ainv[i] = backsub(R,QT[i]);
		       	return Ainv;	
		}
		throw new System.ArgumentException($"Can't invert non square matrix with size: ({A.size1}, {A.size2})");
	}//inv
}//QRGS
 
public static class linsol{

	public static vector TriDiagSol(matrix A, vector b){
		if(A.size1 != A.size2 || A.size2 != b.size){
			throw new System.ArgumentException($"Bad dimensions, A: ({A.size1},{A.size2}), b: {b.size}");}
		int n = b.size;
		for(int i = 1; i < n; i++){ //Do gaussian elimination
			double w = A[i,i-1]/A[i-1,i-1]; //Ai = A[i,i-1], Di = A[i,i], Qi = A[i,i+1]
			A[i,i] -= w*A[i-1,i];
			b[i] -= w*b[i-1];}
		vector sol = new vector(n); //Construct solution
		sol[n-1] = b[n-1]/A[n-1,n-1]
		for(int i = n-2; i > 0; i--)sol[i] = (b[i] - A[i,i+1]*x[i+1])/A[i,i];
		return sol;
	}//TriDiagSol
}//linsol
