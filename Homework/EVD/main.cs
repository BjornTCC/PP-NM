using static System.Console;

public class main{
	public static void Main(){
		int n = 6;
		matrix A = new matrix(n);
		System.Random random = new System.Random();
		for(int i = 0; i < n; i++)
		for(int j = i; j < n; j++){
			A[i,j] = random.NextDouble();
			A[j,i] = A[i,j];
		}
		A.print("A =");
		(vector w, matrix V) = jacobi.cyclic(A);
		matrix D = matrix.diag(w);
		D.print("D approx: ");
		V.print("eigenvectors = ");
		matrix VT = V.transpose();
		matrix VTAV = VT*A*V;
		matrix VDVT = V*D*VT;
		matrix VTV = VT*V;
		VTAV.print("V^T*A*V = ");
                VDVT.print("V*D*VT = ");
                VTV.print("V^T*V = ");
	}//Main
}//main
