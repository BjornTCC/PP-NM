using System;
using static System.Console;
using static System.Math;

namespace root{
	
	public class newton{
		public static readonly double ε = Pow(2,-26), λmin = Pow(2,-10);
		
                public readonly Func<vector,vector> F;
		public vector x, f;
		public int steps, f_eval;
		public bool status;
		
		//constructor
		public newton(
			Func<vector,vector> func, 
			vector x0, 
			int max_steps = 9999,
			double acc = 1e-4)
			{
			F = func; f_eval = 0;
			steps = 0; f_eval = 1;
			x = x0.copy(); f = F(x0); vector f1 = f, Dx = new vector(x0.size);
			status = true;
			do{
			steps++;
			matrix J = jacobian(x,f);
			Dx = QRGS.solve(J, -f);
			double lambda = 1;
			f1 = F(x + Dx); f_eval++;
			while(f1.norm() > (1-lambda/2)*f.norm() && λmin < lambda){
				lambda/=2;
				f1 = F(x+lambda*Dx); f_eval++;
				}
			x+=lambda*Dx;
			f = f1;
			}while(f.norm() >= acc && Dx.norm() >= ε*x.norm() && steps < max_steps);
			if(steps >= max_steps)status = false;
			}
		
		matrix jacobian(
                        vector x,
                        vector f0 = null
                        ){                              //compute the jacobian of a function f
                	if(f0==null){f0 = F(x);f_eval++;}
                	int m = x.size, n = f0.size;
                	matrix jac = new matrix(n,m);
                	vector x2 = x.copy(), df = new vector(n);
                	for(int i=0;i<m;i++){
                        	double dx = ε*Max(1,Abs(x2[i]));
                      		x2[i] += dx;
                        	df = F(x2) - f0; 
				f_eval++;
                        	jac[i] = df/dx;
                        	x2[i] = x[i];}
                	return jac;
		}//jacobian	
	}//newton
	
	public class newton_quad_int{
		public readonly Func<vector,vector> F;
                public static readonly double ε = Pow(2,-26), λmin = Pow(2,-10);

                public vector x, f;
                public int steps, f_eval;
                public bool status;
		
		public newton_quad_int(
			Func<vector,vector> func, 
			vector x0, 
			double acc = 1e-4,
			int max_steps = 9999){
		F = func; f_eval = 1; status = true;
		double lambda = 1; 
		steps = 0; int _steps = 0;
	        x = x0.copy(); vector Dx = new vector(x0.size), f0 = F(x0), f1 = f0;
		double a = 0, b = 0, c = 0;
		do{
                steps++;
		matrix J = jacobian(x,f0);
                Dx = QRGS.solve(J, -f0);
		lambda = 1;
	       	f1 = F(x + Dx);	f_eval++;
		_steps = 0;
		c = 0.5*f0.dot(f0);                             //compute quad. interpolation
                b = f0.dot(J*Dx);
		while(f1.norm() > (1-lambda/2)*f0.norm() && _steps<3){		//compute quad. interpolation
				a = (f1.dot(f1)-c)/(lambda*lambda) - b/lambda;
				if(0.1<-b/(2*a) && -b/(2*a)<=1)lambda = -b/(2*a);	//we wish to have lambda in (0,1]
				else lambda/=2;
				f1 = F(x+lambda*Dx); f_eval++;
				_steps++;
				}
		x+=lambda*Dx;
		f0=f1;
                }while(f0.norm() >= acc && Dx.norm() >= ε*x.norm() && steps <= max_steps);
                if(steps >= max_steps)status = false;
                f = f0;
		}

		matrix jacobian(
                        vector x,
                        vector f0 = null
                        ){                              //compute the jacobian of a function f
                        if(f0==null){f0 = F(x);f_eval++;}
                        int m = x.size, n = f0.size;
                        matrix jac = new matrix(n,m);
                        vector x2 = x.copy(), df = new vector(n);
                        for(int i=0;i<m;i++){
                                double dx = ε*Max(1,Abs(x2[i]));
                                x2[i] += dx;
                                df = F(x2) - f0;
                                f_eval++;
                                jac[i] = df/dx;
                                x2[i] = x[i];}
                        return jac;
                }//jacobian
	}//newton_quad_int
}//root
