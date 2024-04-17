using System;
using static System.Console;
using static System.Math;

class main{
	public static vector gradient_central(Func<vector,double> F, vector x,
					vector f_p = null,
					vector f_m = null
					){
        	int n = x.size; double eps = Pow(2,-26);
		double f_pi = 0, f_mi = 0;
		vector grad = new vector(n), y = x.copy();
                for(int i=0;i<n;i++){
                	double dx=Abs(x[i])*eps;
                        x[i]+=dx;y[i]-=dx;
			if(f_p!=null)f_pi=f_p[i];
			else f_pi = F(x);
			if(f_m!=null)f_mi=f_m[i];
                        else f_mi = F(y);
                        grad[i]=(f_pi-f_mi)/(2*dx);
                        x[i]-=dx;y[i]+=dx;
                }
                return grad;
        }//gradient_forward

	public static matrix hessian_central(Func<vector,double> F,vector x){
        	int n = x.size; double eps = Pow(2,-13);
		matrix H=new matrix(n);
		vector x_pp = x.copy(), x_pm = x.copy(), x_mp = x.copy(), x_mm = x.copy();
                for(int j=0;j<n;j++)
		for(int i=0;i<n;i++){
                	double dxj=Abs(x[j])*eps;
			x_pp[j]+=dxj; x_pm[j]+=dxj;
			x_mp[j]-=dxj; x_mm[j]-=dxj;
			double dxi = Abs(x[i])*eps;
                        x_pp[i]+=dxi; x_pm[i]-=dxi;
                        x_mp[i]+=dxi; x_mm[i]-=dxi;
			H[i,j]=(F(x_pp)-F(x_mp)-F(x_pm)+F(x_mm))/(4*dxj*dxi);
			x_pp[i] = x[i];x_mp[i] = x[i];x_pm[i] = x[i];x_mm[i] = x[i];
                        x_pp[j] = x[j];x_mp[j] = x[j];x_pm[j] = x[j];x_mm[j] = x[j];
                }
                return H;
       	}//hessian_forward
	
	public static (vector,matrix) central_grad_hess(Func<vector,double>F,vector x){
		int n = x.size; double eps = Pow(2,-20);
		matrix H = new matrix(n); vector grad = new vector(n);
		vector x_pp = x.copy(), x_pm = x.copy(), x_mp = x.copy(), x_mm = x.copy();
                for(int j=0;j<n;j++)
                for(int i=0;i<n;i++){
                        double dxj=Max(1,Abs(x[j]))*eps;
                        double dxi=Max(1,Abs(x[i]))*eps;
                        x_pp[j]+=dxj; x_pm[j]+=dxj;
                        x_mp[j]-=dxj; x_mm[j]-=dxj;
                        x_pp[i]+=dxi; x_pm[i]-=dxi;
                        x_mp[i]+=dxi; x_mm[i]-=dxi;
			double F_pp = F(x_pp), F_pm = F(x_pm), F_mp = F(x_mp), F_mm = F(x_mm);
                        H[i,j]=(F_pp-F_mp-F_pm+F_mm)/(4*dxj*dxi);
			if(i==j) grad[j] = (F_pp-F_mm)/(4*dxj);
                        x_pp[i] = x[i];x_mp[i] = x[i];x_pm[i] = x[i];x_mm[i] = x[i];
                        x_pp[j] = x[j];x_mp[j] = x[j];x_pm[j] = x[j];x_mm[j] = x[j];
		}
		return(grad,H);
	}//grad_hess_central	
	public static int Main(){
		int N = 3;
		System.Random random = new System.Random();
		matrix A = new matrix(N); vector y = new vector(N);
		for(int i = 0; i < N; i++)
		for(int j = i; j < N; j++){ 
			y[i] = random.NextDouble();
			A[i,j] = random.NextDouble();
			A[j,i] = A[i,j];
		}
		vector c = A*y;
		Func<vector,double> quad = x=>0.5*x.dot(A*x);
		(vector grad,matrix H) = central_grad_hess(quad,y); 
		
		y.print("x=");
		if(A.approx(H,0.0001) && vector.approx(c,grad,0.0001))WriteLine("Test succesful");
		else WriteLine("Test failed");
		c.print("c=");
		grad.print("gradient=");
		A.print("A=");
		H.print("Hessian=");
	
		return 1;
	}//Main
}//main
