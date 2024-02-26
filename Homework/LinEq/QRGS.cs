using static System.Math;

public static class QRGS{
	
	public static (matrix, matrix) decomp(matrix A){ //returns in format [Q, R]
		int m = A.size1;
		int n = A.size2;
		matrix Q=A.copy(), R=new matrix(m,m);
		for (int i =0; i<m; i++){
			R[i,i]=matrix.norm(Q[i]);
			for(int k = 0; k < n; k++)Q[i][k]/=R[i,i];
			for (int j=i+1; j<m; j++){
				R[i,j]=matrix.dot(Q[i],Q[j]);
				for(int k = 0; k < n; k++)Q[j][k] -= Q[i][k]*R[i,j];
			}}
		return (Q, R);
	}//decomp	
}//QRGS
