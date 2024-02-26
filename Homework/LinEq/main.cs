static class main{
	static int Main(){
		matrix A = new matrix("1 0 1; 0 1 0; 1 0 1");
		(matrix Q, matrix R) = QRGS.decomp(A);
		Q.print("Q = ");
		R.print("R = ");
		return 0;
	}//Main
}//main
