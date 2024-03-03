using static System.Math;

public static class fit{
	
	public static (vector,matrix) lsfit(System.Func<double,double>[] fs, vector x, vector y, vector dy){
		if(x.size != y.size || x.size != dy.size || y.size != dy.size){
			throw new System.ArgumentException($"Incompatible data sizes: ({x.size} {y.size} {dy.size}).");}
		matrix A = new matrix(x.size, fs.Length);
		vector b = new vector(y.size);
		for(int i = 0; i < x.size; i++){
			b[i] = y[i]/dy[i];
			for(int k = 0; k < fs.Length; k++)A[i,k] = fs[k](x[i])/dy[i];}
		vector c = QRGS.solve(A, b);
		matrix ATA = A.transpose()*A;
		matrix cov = QRGS.inv(ATA);
		return (c,cov);
	}//lsfit
}//lsfit
