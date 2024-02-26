using static System.Console;

static class main{
	static int Main(){
		System.Random random = new System.Random();
		int m = 5, n = 3;
		matrix A = new matrix(m,n);
		for(int i = 0; i < m; i++){
		for(int j = 0; j < n; j++) A[i,j] = random.NextDouble();}
		A.print("A = ");
		(matrix Q, matrix R) = QRGS.decomp(A);
		Q.print("Q = ");
		R.print("R = ");
		matrix QQ = Q.transpose()*Q;
		QQ.print("QQ^T =");
		matrix QR = Q*R;
		QR.print("QR =");

		matrix B = new matrix(m);
		double[] b = new double[m];
                for(int i = 0; i < m; i++){ 
			b[i] = random.NextDouble();
                	for(int j = 0; j < m; j++) B[i,j] = random.NextDouble();}
		B.print("B = ");
		WriteLine("b = [{0}]", string.Join(",", b));
		double[] sol = QRGS.solve(B, b);
                WriteLine("x = [{0}]", string.Join(",", sol));
		double[] Bx = B*sol;
                WriteLine("Bx = [{0}]", string.Join(",", Bx));

		WriteLine("Matrix inverse part:");
		WriteLine("");
		matrix C = new matrix(m);
                for(int i = 0; i < m; i++){
                for(int j = 0; j < m; j++) C[i,j] = random.NextDouble();}
		matrix Cinv = QRGS.inv(C);
		C.print("C = ");
		Cinv.print("C^-1 = ");
		matrix CCinv = C*Cinv;
		CCinv.print("C*C^-1 = ");
		return 0;
	}//Main
}//main
