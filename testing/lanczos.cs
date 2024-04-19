using static System.Console;
using static System.Math;

public class main{
	
	public class lanczos{
		
		public readonly int dim;
		public readonly matrix A, TD, V; 

		//constructor
		public lanczos(matrix B, int m = -1,double zero =1e-10){
			if(B.size1 != B.size2) throw new System.ArgumentException($"lanczos: Non-square matrix of size {A.size1}, {A.size2}");
			dim = B.size1; 
			A = B.copy(); 
			if(m==-1) m = dim;
			V = new matrix(dim); TD = new matrix(dim);
			double α, β;
			//initial iteration step, see wikipedia
			V[0] = random_orthog(V[0],0);
			vector w = A*V[0];
			α = w.dot(V[0]);
			w = w - α*V[0];
			TD[0,0] = α;

			//general iteration step:
			for(int j=1;j<m;j++){
				β = w.norm();
				if(β < zero) V[j] = random_orthog(V[j],j);
				else V[j] = w/β;
				w = A*V[j];
				α = w.dot(V[j]);
				w = w - α*V[j] - β*V[j-1];
				TD[j,j] = α; TD[j,j-1] = β; TD[j-1,j] = β;
				}
			}
		
		public vector random_orthog(vector v, int i){
			System.Random random = new System.Random();
			for(int k=0;k<dim;k++) v[k] = 2*random.NextDouble()-1;
			for(int k=0;k<i;k++) v = v - v.dot(V[k])*V[k];
			return v/v.norm();
		}	
	}//lanczos

	public static int Main(){
		int N = 7;
		System.Random random = new System.Random();
		matrix A = new matrix(N);
		for(int i = 0; i < N; i++)
		for(int j = i; j < N; j++){ 
			A[i,j] = random.NextDouble();
			A[j,i] = A[i,j];
		}
		lanczos L = new lanczos(A);
	       	A.print("A = ");
		matrix V = L.V,	VT = V.transpose(), TD = L.TD;
		V.print("V=");
		TD.print("T = ");
		matrix VVT = V*VT, VTV = VT*V;
		VVT.print("V*VT=");
		VTV.print("VT*V=");
		matrix res = V*TD*VT;
		res.print("V*T*VT=");
		if(res.approx(A)&&VVT.approx(matrix.id(N))&&VTV.approx(matrix.id(N))) WriteLine("Test succes");
		else WriteLine("Test failed");	
		return 0;
	}//Main
}//main
