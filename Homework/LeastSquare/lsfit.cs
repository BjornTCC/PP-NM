using static System.Math;

public static class fit{
	
	public static (vector,matrix) lsfit(System.Func<double,double>[] fs, vector x, vector y, vector dy){
		if(x.size != y.size || x.size != dy.size || y.size != dy.size){
			throw new System.ArgumentException($"lsfit: Incompatible data sizes: ({x.size} {y.size} {dy.size}).");}
		matrix A = new matrix(x.size, fs.Length);
		vector b = new vector(y.size);
		for(int i = 0; i < x.size; i++){
			b[i] = y[i]/dy[i];
			for(int k = 0; k < fs.Length; k++)A[i,k] = fs[k](x[i])/dy[i];}
		(matrix Q, matrix R) = QRGS.decomp(A);
		vector c = QRGS.backsub(R, Q.transpose()*b);
		matrix Rinv = QRGS.inv(R);
		matrix cov = Rinv*Rinv.transpose();
		return (c,cov);
	}//lsfit
}//lsfit
