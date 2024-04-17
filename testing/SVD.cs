using static System.Console;
using static System.Math;

public static class main{

	public class SVD{
	
	public int steps,dim1,dim2;
	public matrix A,U,D,V;

	//constructor
	
	public SVD(matrix B, string method = "one-sided", double acc = System.Double.NaN, int max_steps = 9999){
		if(method == "one-sided") SVD_one_sided(B,acc,max_steps);
		else if(method == "two-sided") SVD_two_sided(B,acc,max_steps);
		else throw new System.ArgumentException("SVD: Invalid method: "+method);
	}

	public void SVD_one_sided(matrix B,double acc, int max_steps){
		if(System.Double.IsNaN(acc)) acc = Pow(2,-52);
		steps = 0; dim1 = B.size1; dim2 = B.size2;
		A=B; matrix A_new = A.copy();
		D = matrix.id(dim1); V = matrix.id(dim2);
		bool changed;
		do{
			changed = false;			// Assume success
			steps++;
			for(int p=0;p<dim2;p++)
			for(int q=p+1;q<dim2;q++){
				double θ = 0.5*Atan2(2*A_new[p].dot(A_new[q]),A_new[q].dot(A_new[q]) - A_new[p].dot(A_new[p]));
				timesJ(A_new,p,q,θ);
				timesJ(V,p,q,θ);
				double Dp = A_new[p].norm(), Dq = A_new[q].norm();
				if(Abs(D[p,p]-Dp)>acc || Abs(D[q,q]-Dq)>acc)changed = true;	// check convergence
				D[p,p] = Dp; D[q,q] = Dq;
			}
		}while(changed && steps < max_steps);
		U = A_new.copy(); for(int i=0;i<dim2;i++)U[i]/=D[i,i];	
	}
	
	public void SVD_two_sided(matrix B,double acc, int max_steps){
                if(System.Double.IsNaN(acc)) acc = Pow(2,-52);
                steps = 0; dim1 = B.size1; dim2 = B.size2;
                A=B; matrix A_new = A.copy();
                D = new matrix(dim2); V = matrix.id(dim2); U = matrix.id(dim1);
                bool changed;
                do{
                        changed = false;                        // Assume success
                        steps++;
                        for(int p=0;p<dim1;p++)
                        for(int q=p+1;q<dim2;q++){
				double θG = Atan2(A_new[p,q]-A_new[q,p],A_new[p,p]+A_new[q,q]);
				Jtimes(A_new,p,q,-θG);
                                double θJ = 0.5*Atan2(A_new[p,q]+A_new[q,p], A_new[q,q] - A_new[p,p]);
                                timesJ(A_new,p,q,θJ);
				Jtimes(A_new,p,q,-θJ);
				A_new.print(file:System.Console.Error);
				timesJ(U,p,q,θG);
				timesJ(U,p,q,θJ);
                                timesJ(V,p,q,θJ);
                                double Dp = A_new[p,p], Dq = A_new[q,q];
				if(Dp<0){Dp*=-1;U[p]*=-1;}
                                if(Dq<0){Dq*=-1;U[q]*=-1;}
                                if(Abs(D[p,p]-Dp)>acc || Abs(D[q,q]-Dq)>acc)changed = true;     // check convergence
                                D[p,p] = Dp; D[q,q] = Dq;
                        }
                }while(changed && steps < max_steps);
        }

	static void timesJ(matrix A, int p, int q, double θ){
		double c = Cos(θ), s = Sin(θ);
		for(int i = 0; i < A.size1; i++){
			double Aip = A[i,p], Aiq = A[i,q];
			A[i,p] = c*Aip - s*Aiq;
			A[i,q] = c*Aiq + s*Aip;
			}
	}//timesJ
	
	static void Jtimes(matrix A, int p, int q, double theta){
                double c = Cos(theta), s = Sin(theta);
                for(int i = 0; i < A.size2; i++){
			double Api = A[p,i], Aqi = A[q,i];
                        A[p,i] = c*Api + s*Aqi;
                        A[q,i] = c*Aqi - s*Api;
                        }
	}//Jtimes

	}//SVD

	public static int Main(){
		int n = 5, m = 5;
                //Create a random matrix
                matrix A = new matrix(n,m);
                System.Random random = new System.Random();
                for(int i = 0; i < n; i++)
                for(int j = 0; j < m; j++){
                        A[i,j] = 2*random.NextDouble()-1;
                }
                //test decomposition
                A.print("A =");
                SVD A_SVD = new SVD(A,method:"two-sided");
		WriteLine($"Steps: {A_SVD.steps}");
                matrix D = A_SVD.D, U = A_SVD.U, V = A_SVD.V;
		D.print("D=");
                U.print("U="); matrix UTU = U.transpose()*U;	
		V.print("V="); matrix VTV = V.transpose()*V, VVT = V*V.transpose();
		UTU.print("UT*U");
                VTV.print("VT*V");
                VVT.print("V*VT");

                matrix res = U*D*V.transpose();
                res.print("U*D*VT=");
                if(res.approx(A)) WriteLine($"UDV^T = A: Test success");
                else WriteLine($"UDV^T = A: Test Failure");
                WriteLine("--------------------------------------------------------------");
		return 0;
	}//Main
}//main
