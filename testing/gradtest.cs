using System;
using static System.Console;
using static System.Math;

class main{
	public static vector gradient_forward(Func<vector,double> F, vector x,double fx = Double.NaN){
        	int n = x.size; double eps = Pow(2,-26);
		vector grad = new vector(n);
                if(Double.IsNaN(fx))fx = F(x); /* no need to recalculate at each step */
                for(int i=0;i<n;i++){
                	double dx=Max(1,Abs(x[i]))*eps;
                        x[i]+=dx;
                        grad[i]=(F(x)-fx)/dx;
                        x[i]-=dx;
                }
                return grad;
        }//gradient_forward α β γ λ φ

	public static matrix hessian_forward(Func<vector,double> F,vector x,double fx = Double.NaN,vector gradx = null){
        	int n = x.size; double eps = Pow(2,-13);
		matrix H=new matrix(n);
                if(Double.IsNaN(fx))fx = F(x);
		//WriteLine($"f(x) = {fx}");
                if(gradx==null) gradx=gradient_forward(F,x,fx);
		//gradx.print("grad(x) = ");
                for(int j=0;j<n;j++){
                	double dx=Max(1,Abs(x[j]))*eps;
			//WriteLine($"dx={dx}");
                        x[j]+=dx;
			//x.print("x=");
                        vector dgrad=gradient_forward(F,x)-gradx;
			//dgrad.print("dgrad");
                        for(int i=0;i<n;i++) H[j,i]=dgrad[i]/dx;
                        x[j]-=dx;
			//x.print("x=");
                }
                return H;
                //return 0.5*(H+H.transpose()); // you think?
       	}//hessian_forward
	
	public static int Main(){
		int N = 3;
		double acc = 0.0001;
		System.Random random = new System.Random();
		matrix A = new matrix(N); vector c = new vector(N), y = new vector(N);
		for(int i = 0; i < N; i++)
		for(int j = i; j < N; j++){ 
			c[i] = random.NextDouble();
			y[i] = random.NextDouble();
			A[i,j] = random.NextDouble();
			A[j,i] = A[i,j];
		}
		Func<vector,double> quad = x => 0.5*(x-c).dot(A*(x-c));
		Func<vector,double> lin = x => x.dot(c);
		matrix H = hessian_forward(quad,y);
		vector grad = gradient_forward(lin,y);
		y.print("y=");
		A.print("A=");
		H.print("H=");
		gradient_forward(quad,y).print("             grad=");
		vector tmp=A*(y-c);
 		tmp	                .print("analytic gradient=");
		
		y.print("x=");
		if(A.approx(H,acc) && vector.approx(c,grad,acc))WriteLine($"Test succesful within accuracy: {acc}");
		else WriteLine($"Test failed within accuracy: {acc}");
		c.print("c=");
		grad.print("gradient=");
	
		return 1;
	}//Main
}//main
