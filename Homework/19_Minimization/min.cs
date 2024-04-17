using System;
using static System.Math;

namespace min{

	public class newton{
		public static readonly double ε = Pow(2,-26);
		public static readonly double ε4 = Pow(2,-13);
		public static readonly double λmin=Pow(2,-13);

                public readonly Func<vector,double> F;  /* objective function */
                public readonly int n;                  /* dimension */
                public vector x;                        /* found minimum */
                public double f;                        /* value of f at minimum */
                public int steps;                       /* no. of steps taken */
                public int f_eval;                      /* no. of function evaluations */
                public bool status;                     /* whether convergence was achieved within alloted steps*/
		bool _central;

                //Constructor
                public newton(Func<vector,double> func,	/* objective function */
                vector start,                           /* starting point */
                double acc = 0.001,                     /* accuracy goal, on exit |gradient| should be < acc */
                int max_steps = 9999,			/* Maximum allowed steps */
		bool central = false			/* whether to use central difference formula */
                ){
		_central = central;
                F=func; n = start.size; steps = 0; f_eval = 0;
                x = start.copy();
        	double lambda = 1, fx = 0, fy = 0;
		
		do{
			steps++;
			fx = F(x); f_eval++;
			(vector gradx, matrix H) = grad_hess(x,fx);
			if(gradx.norm() < acc) break;
			vector Dx = QRGS.solve(H, -gradx);
			lambda = 1;
			do{
				fy = F(x+lambda*Dx); f_eval++;
				if(fy < fx ) break; /* step acceptance */
				if(lambda < λmin) break; /* step acceptance */
				lambda /= 2;
			}while(true);
			fx = fy; x+=lambda*Dx;
		}while(steps < max_steps);
                f = fx;
                if(steps >= max_steps)status = false;
                else status = true;
                }
		
		//numerical gradients and hessian

		vector gradient_forward(vector x,double fx = Double.NaN){
			vector grad = new vector(n);
			if(Double.IsNaN(fx)){fx = F(x); f_eval++;} 	/* no need to recalculate at each step */
			for(int i=0;i<n;i++){
				double dx=Abs(x[i])*ε;
				x[i]+=dx;
				grad[i]=(F(x)-fx)/dx; 
				f_eval++;
				x[i]-=dx;
			}
			return grad;
		}//gradient_forward

		matrix hessian_forward(vector x,double fx = Double.NaN,vector gradx = null){
			matrix H=new matrix(n);
                        if(Double.IsNaN(fx)){fx = F(x); f_eval++;}
			if(gradx==null) gradx=gradient_forward(x,fx);
			for(int j=0;j<n;j++){
				double dx=Abs(x[j])*ε4;
				x[j]+=dx;
				vector dgrad=gradient_forward(x)-gradx;
				for(int i=0;i<n;i++) H[i,j]=dgrad[i]/dx;
				x[j]-=dx;
			}
			//return H;
			return 0.5*(H+H.transpose()); // you think?
		}//hessian_forward
		
		(vector,matrix) grad_hess(vector x, double fx = Double.NaN){
			if(_central) return central_grad_hess(x,fx);
			else return forward_grad_hess(x,fx);
		}//grad_hess
		
		(vector,matrix) forward_grad_hess(vector x, double fx = Double.NaN){
			vector gradx = gradient_forward(x,fx);
			return(gradx,hessian_forward(x,fx,gradx));
		}//forward_grad_hess

		(vector,matrix) central_grad_hess(vector x,double fx = Double.NaN){
			double F_pp = 0, F_mp = 0, F_pm = 0, F_mm = 0;
			if(Double.IsNaN(fx))fx = F(x);
			matrix H = new matrix(n); vector grad = new vector(n);
			vector x_pp = x.copy(), x_pm = x.copy(), x_mp = x.copy(), x_mm = x.copy();
                	for(int j=0;j<n;j++)
	                for(int i=0;i<n;i++){
        	                double dxi=Abs(x[i])*ε4;
                	        double dxj=Abs(x[j])*ε4;
	                        x_pp[j]+=dxj; x_pm[j]+=dxj;
        	                x_mp[j]-=dxj; x_mm[j]-=dxj;
                	        x_pp[i]+=dxi; x_pm[i]-=dxi;
                        	x_mp[i]+=dxi; x_mm[i]-=dxi;
				F_pp = F(x_pp); F_mm = F(x_mm); f_eval+=2;
                                if(i==j){
					grad[j] = (F_pp-F_mm)/(4*dxj); 
					F_mp = fx; F_pm = fx;
				       	H[i,j]=(F_pp+F_mm-2*F_mp)/(4*dxi*dxj);	
					}
				else{	F_pm = F(x_pm); F_mp = F(x_mp);f_eval+=2;
                        		H[i,j]=(F_pp-F_mp-F_pm+F_mm)/(4*dxj*dxi);}
                        	x_pp[i] = x[i];x_mp[i] = x[i];x_pm[i] = x[i];x_mm[i] = x[i];
                        	x_pp[j] = x[j];x_mp[j] = x[j];x_pm[j] = x[j];x_mm[j] = x[j];
			}
			return(grad,H);
		}//grad_hess_central
	}//newton
	

	// From here on methods used for the homework ends
	// I implemented qnewton and nelder-mead based on old homework 
	// just for the fun of it

	public class qnewton{ 
		static readonly double λmin=Pow(2,-26);
		
		public readonly Func<vector,double> F;	/* objective function */
		public readonly int n; 			/* dimension */
		public vector x;	 		/* found minimum */
		public double f;			/* value of f at minimum */
		public int steps; 			/* no. of steps taken */
		public int f_eval;			/* no. of function evaluations */
		public bool status;			/* whether convergence was achieved within alloted steps*/

		//Constructor
		public qnewton(Func<vector,double> func,/* objective function */
		vector start, 				/* starting point */
		double acc = 0.001, 			/* accuracy goal, on exit |gradient| should be < acc */
		int max_steps = 9999			/* Maximum allowed steps */
		){
		F=func; n = start.size; steps = 0; f_eval = 0;
		matrix B = matrix.id(n);					//Hessian inverse estimate
		vector gradf = new vector(n), Dx = new vector(n); 
		x = start.copy();
		double lambda = 1, fx = 0, fy = 0, gamma = 0, sy = 0;		//dummy variables for the algorithm
		vector y = new vector(n), u = new vector(n), a = new vector(n); //parameters for update

		do{
			steps++;
			fx = F(x); f_eval++;
			gradf = grad(x,fx);
			Dx = -B*gradf;
			lambda = 1;
			while(true){
				fy = F(x+lambda*Dx); f_eval++;
				if(fy<fx){
					x+=lambda*Dx; 			//accept step
					y = grad(x,fy)-gradf;	//update B using symmetric Broydens update
					sy = lambda*Dx.dot(y);
					if(Abs(sy) > Pow(10,-6)){	//update only if denominator is sufficiently large
						u = lambda*Dx - B*y;
						gamma = u.dot(y)/(2*sy);
						a = (u-gamma*lambda*Dx)/sy;
						B.sym2update(a,Dx,lambda);
					}
					break;
				}
				lambda/=2;
				if(lambda < λmin){
					x+=lambda*Dx;
					B = matrix.id(n);
					break;}
			}
		}while(gradf.norm()>acc && steps < max_steps);
		f = fx;
		if(steps >= max_steps)status = false;
		else status = true;
		}

	        vector grad(vector x, double f0 = double.NaN){
        	        if(f0 == double.NaN) f0 = F(x);
			double eps = Pow(2,-26);
               		vector res = new vector(n), y = x.copy();
               		for(int i=0;i<n;i++){
                        	double dx = Abs(x[i])*eps;
                        	y[i]+=dx;
                        	res[i] = (F(y)-f0)/dx;
				f_eval++;
                        	y[i]=x[i];}
                	return res;
        	}//grad
	}//qnewton
	
	public class downhill_sim{
			public readonly Func<vector,double> F;  /* objective function */
                	public readonly int n;                  /* dimension */
                	public vector x;                        /* found minimum */
	                public double f;                        /* value of f at minimum */
        	        public int steps;                       /* no. of steps taken */
	                public int f_eval;                      /* no. of function evaluations */
        	        public bool status;                     /* whether convergence was achieved within alloted steps*/
			
			//constructor
			public downhill_sim(Func<vector,double> func,	/* objective function */
                	vector start,                         		/* starting point */
                	double acc = 1e-10,                    	 	/* accuracy goal, on exit std should be < acc */
                	int max_steps = 9999,				/* Maximum allowed steps */
			double d = 5					/* starting spread */
			){
			F = func;
			steps = 0; status = false;
			simplex simp = new simplex(F,start,d:d);
			do{
			simp.update_op_vals();
			if(simp.ref_val < simp.minval){
				if(simp.exp_val < simp.ref_val)simp.expansion();
				else simp.reflection();
			}
			else{
				if(simp.ref_val < simp.maxval)simp.reflection();
				else if(simp.con_val < simp.maxval)simp.contraction();
				else simp.reduction();
			}
			steps++;
			}while(simp.std()>acc && steps < max_steps);
			if(steps < max_steps) status = true;
			x = simp.min();
			f = simp.minval;
			f_eval = simp.simp_f_eval;
			}//constructor

			class simplex{
			public readonly int dim;
			public int nmax, nmin, simp_f_eval;
			public vector[] points;
			public vector phi_vals;
			public readonly Func<vector,double> phi;
			public vector centroid;
			public double maxval, minval, ref_val, exp_val, con_val;

			//constructor
			public simplex(Func<vector,double> f, vector x, double d=5){
				dim = x.size; phi = f; nmax = 0; nmin = 0;
				vector[] ps = new vector[dim+1]; ps[0] = x.copy();
				for(int i=1;i<dim+1;i++){ps[i]=x.copy();ps[i][i-1]+=d;} //create by adding a point in each dimension
				points = ps; simp_f_eval = 0;
        	               	phi_vals = new double[dim+1]; 
				update_phi_vals(); update_cent();
			}

			//update data
			void update_phi_vals(){
				for(int i=0;i<dim+1;i++)phi_vals[i] = phi(points[i]);
				simp_f_eval += dim+1;
				nmin = 0; nmax = 0; maxval = phi_vals[0]; minval = phi_vals[0];
				for(int i=1;i<dim+1;i++){
				if(phi_vals[i]<minval){nmin=i; minval = phi_vals[i];}
				if(phi_vals[i]>maxval){nmax=i; maxval = phi_vals[i];}
				}
			}

			void update_max(){
				nmax = 0; maxval = phi_vals[0];
				for(int i=1;i<dim+1;i++){
				if(phi_vals[i]>maxval){nmax=i; maxval = phi_vals[i];}
				}
			}

			void update_cent(){
				centroid = new vector(dim);
				for(int i=0;i<dim+1;i++){
					if(i!=nmax)centroid+=points[i]/dim;
				}
			}
			
			public void update_op_vals(){
				ref_val = phi(2*centroid - points[nmax]);
				exp_val = phi(3*centroid - 2*points[nmax]);
				con_val = phi(centroid/2 - points[nmax]/2);
				simp_f_eval += 3;
			}
			
			//getters and setters

			public vector min(){return points[nmin];}
			
			//operations on simplex

			public void reflection(){
				points[nmax] = 2*centroid - points[nmax];
                                phi_vals[nmax] = ref_val;
				if(ref_val < minval){nmin=nmax; minval = ref_val;}
				update_max(); update_cent();
			}

			public void expansion(){
				points[nmax] = 3*centroid - 2*points[nmax];
                                phi_vals[nmax] = exp_val;
                                if(exp_val < minval){nmin=nmax; minval = exp_val;}
                                update_max(); update_cent();
			}


			public void contraction(){
	                        points[nmax] = centroid/2 - points[nmax]/2;
                                phi_vals[nmax] = con_val;
                                if(con_val < minval){nmin=nmax; minval = con_val;}
                                update_max(); update_cent();
        	        }
				
			public void reduction(){
				for(int i=0;i<dim+1;i++){
					if(i!=nmin)points[i]=(points[i]+points[nmin])/2;
				}
				update_phi_vals(); update_cent();
			}
	
			//volume
			public double std(){					//compute standard deviation of function vals
				double mean = 0, mean_sq = 0;
				for(int i=0;i<dim+1;i++){
					mean += phi_vals[i]/(dim+1);
					mean_sq += Pow(phi_vals[i],2)/(dim+1);
				}
				return mean_sq - mean*mean;
			}
		}//simplex

	}//downhill_sim
}//min
