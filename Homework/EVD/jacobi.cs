using static System.Math;

public static class jacobi{
	public static void timesJ(matrix A, int p, int q, double theta){
		double c = Cos(theta), s = Sin(theta);
		for(int i = 0; i < A.size1; i++){
			A[i,p] = c*A[i,p] - s*A[i,q];
			A[i,q] = c*A[i,q] + s*A[i,p];
			}
	}//timesJ
	
	public static void Jtimes(matrix A, int p, int q, double theta){
                double c = Cos(theta), s = Sin(theta);
                for(int i = 0; i < A.size2; i++){
                        A[p,i] = c*A[p,i] + s*A[q,i];
                        A[q,i] = c*A[q,i] - s*A[p,i];
                        }
	}//Jtimes
	 
	public static (vector, matrix) cyclic(matrix A){
		matrix D = A.copy(), V = matrix.id(A.size1);	
		vector w = new vector(D.size1);
		bool changed;
		do{
			changed = false; //assume succesful diagonalization until proven otherwise
			for(int p = 0; p < D.size1-1; p++) //loop over upper triangle part of matrix to do rotation
			for(int q = p+1; q < D.size2; q++){
                                double theta = 0.5*Atan2(2*D[p,q], D[q,q] - D[p,p]); //find theta
				double c = Cos(theta), s = Sin(theta);
				double new_Dpp = c*c*D[p,p] - 2*s*c*D[p,q] + s*s*D[q,q]; //find new diagonal elements
				double new_Dqq = c*c*D[q,q] + 2*s*c*D[p,q] + s*s*D[p,p];
				if(new_Dpp != D[p,p] || new_Dqq != D[q,q]){
                                	timesJ(D, p, q, theta);
                                	Jtimes(D, p ,q, -theta); //D <- J^T*D*J
                                	timesJ(V, p, q, theta); //V <- VJ
					changed = true;} //proven otherwise
				}
		}while(changed);
		for(int i = 0; i < w.size; i++)w[i] = D[i,i]; //collect the eigenvalues as the diagonal elements of D
		return (w, V);
	}//cyclic
}//jacobi
