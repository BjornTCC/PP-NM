using System;
using static System.Math;

public class root{

	static matrix jacobian(Func<vector,vector> f, vector x){ //compute the jacobian of a function f
		vector f0 = f(x);
		int m = x.size, n = f0.size;
		matrix jac = new matrix(n,m);
		double eps = Pow(2,-26), dx = 1;
		vector x2 = x.copy(), df = new vector(n);
		for(int i=0;i<m;i++){
			dx = eps*(Abs(x2[i])+1e-4);
			x2[i] += dx;
			df = f(x2) - f0;
			jac[i] = df/dx;
			x2[i] = x[i];}
		return jac;
	}//jacobian
	
	public static vector newton(Func<vector,vector> f, vector x, double eps = 1e-3){
		double lambda = 1;
		int m = x.size, n = f(x).size;
		vector x0 = x.copy(), Dx = x.copy();
		do{
		vector f0 = f(x0);
		matrix J = jacobian(f,x0);
		Dx = QRGS.solve(J, -f0);
		lambda = 1;
		while(f(x0 + lambda*Dx).norm() > (1-lambda/2)*f0.norm() && 1 < lambda*1024){lambda/=2;}
		x0 +=lambda*Dx;
		}while(f(x0).norm() >= eps && Dx.norm() >= Pow(2,-26)*x0.norm());
		return x0;
	}//newton
}//root
