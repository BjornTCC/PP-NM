using static System.Math;

public static class QRGS{
	
	public static matrix[] decomp(matrix A){
		matrix Q=A.copy(), R=new matrix(m,m);
		for (int i =0; i<m; i ++){
			R[i,i]=matrix.norm(Q[i])
			Q[i]/=R[i,i];
			for (int j=i +1; j<m; j++){
				R[i,j]=Q[i].dot(Q[j]);
				Q[j]−=Q[i]∗R[i,j]; }}
	}//decomp	
}//QRGS
