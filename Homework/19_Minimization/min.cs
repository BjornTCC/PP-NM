using System;
using static System.Math;

public class min{

	public class newton{
		public static readonly double ε = Pow(2,-26);
		public static readonly double λmin=Pow(2,-24);

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
			if(Double.IsNaN(fx)){fx = F(x); f_eval++;} /* no need to recalculate at each step */
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
			double sε = Pow(ε,0.5);
			matrix H=new matrix(n);
                        if(Double.IsNaN(fx)){fx = F(x); f_eval++;}
			if(gradx==null) gradx=gradient_forward(x,fx);
			for(int j=0;j<n;j++){
				double dx=Abs(x[j])*sε;
				x[j]+=dx;
				vector dgrad=gradient_forward(x)-gradx;
				for(int i=0;i<n;i++) H[i,j]=dgrad[i]/dx;
				x[j]-=dx;
			}
			//return H;
			return 0.5*(H+H.transpose()); // you think?
		}//hessian_forward
		
		(vector,matrix) grad_hess(vector x, double fx = Double.NaN){
			if(_central) return central_grad_hess(x);
			else return forward_grad_hess(x,fx);
		}//grad_hess
		
		(vector,matrix) forward_grad_hess(vector x, double fx = Double.NaN){
			vector gradx = gradient_forward(x,fx);
			return(gradx,hessian_forward(x,fx,gradx));
		}//forward_grad_hess

		(vector,matrix) central_grad_hess(vector x,double fx = Double.NaN){
			double sε = Pow(ε,0.5);
			double F_pp = 0, F_mp = 0, F_pm = 0, F_mm = 0;
			if(Double.IsNaN(fx))fx = F(x);
			matrix H = new matrix(n); vector grad = new vector(n);
			vector x_pp = x.copy(), x_pm = x.copy(), x_mp = x.copy(), x_mm = x.copy();
                	for(int j=0;j<n;j++)
	                for(int i=0;i<n;i++){
        	                double dxj=Abs(x[j])*sε;
                	        double dxi=Abs(x[i])*sε;
	                        x_pp[j]+=dxj; x_pm[j]+=dxj;
        	                x_mp[j]-=dxj; x_mm[j]-=dxj;
                	        x_pp[i]+=dxi; x_pm[i]-=dxi;
                        	x_mp[i]+=dxi; x_mm[i]-=dxi;
				F_pp = F(x_pp); F_mm = F(x_mm); f_eval+=2;
                                if(i==j){
					grad[j] = (F_pp-F_mm)/(4*dxj); 
					F_mp = fx; F_pm = fx; 
					f_eval+=2;}
				else{F_pm = F(x_pm); F_mp = F(x_mp);}
				f_eval+=2;
                        	H[i,j]=(F_pp-F_mp-F_pm+F_mm)/(4*dxj*dxi);
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
	
	class simplex{
		public readonly int dim;
		public int nmax, nmin;
		public vector[] points;
		public readonly Func<vector,double> phi;
		public vector centroid;
		public double maxval, minval;

		//constructors
		public simplex(Func<vector,double> f, vector[] ps){
			dim = ps.Length -1; nmax = 0; nmin = 0;
			if(dim!=ps[0].size) throw new ArgumentException($"simplex: Must have {dim + 1} points for dimension {dim}.");
			points = ps; phi = f;
			this.update_max(); this.update_min(); this.update_cent();	
		}

		public simplex(Func<vector,double> f, vector x, double d=5){
			dim = x.size; phi = f; nmax = 0; nmin = 0;
			vector[] ps = new vector[dim+1]; ps[0] = x.copy();
			for(int i=1;i<dim+1;i++){ps[i]=x.copy();ps[i][i-1]+=d;} //create by adding a point in each dimension
			points = ps;
                        this.update_max(); this.update_min(); this.update_cent();
		}
		
		//update data
		void update_max(){
			nmax = 0;
			for(int i=1;i<dim+1;i++){
				if(phi(points[i])>phi(points[nmax])) nmax = i;}
			maxval = phi(points[nmax]);
		}

		void update_min(){
                        nmin = 0;
                        for(int i=1;i<dim+1;i++){
				if(phi(points[i])<phi(points[nmin])) nmin = i;}
                	minval = phi(points[nmin]);
		}

		void update_cent(){
			centroid = new vector(dim);
			for(int i=0;i<dim+1;i++){
				if(i!=nmax)centroid+=points[i]/dim;
			}
		}
		
		//getters and setters
		public vector this[int c]{
			get{return points[c];}
			set{points[c]=value;}
		}

		public simplex copy(){
			simplex c = new simplex(phi, points); return c;
		}

		public vector max(){return points[nmax];}
		
		public vector min(){return points[nmin];}
		
		//operations on simplex

		public void reflection(){
			points[nmax] = 2*centroid - points[nmax];
                        this.update_max(); this.update_min(); this.update_cent();
		}
		
		public double ref_val(){return phi(2*centroid - points[nmax]);}

		public void expansion(){
			points[nmax] = 3*centroid - 2*points[nmax];
                        this.update_max(); this.update_min(); this.update_cent();
		}
		
		public double exp_val(){return phi(3*centroid - 2*points[nmax]);}

		public void contraction(){
                        points[nmax] = 3*centroid/2 - points[nmax]/2;
                        this.update_max(); this.update_min(); this.update_cent();
                }
		public double con_val(){return phi(3*centroid/2 - points[nmax]/2);}

		public void reduction(){
			for(int i=0;i<dim+1;i++){
				if(i!=nmin)points[i]=(points[i]+points[nmin])/2;
			}
		}

		//volume
		public double volume(){
			matrix M = new matrix(dim);
			for(int i=0;i<dim;i++)M[i]=points[i]-points[dim];
			int fact = 1;
			for(int i=1;i<=dim;i++)fact*=i;
			return Abs(QRGS.det(M))/fact;
		}
	}//simplex
	
	public static (vector,double,int) downhill_sim(
			Func<vector,double> f, /* objective function */
			vector start, /* starting point */
			double acc = 0.001
			){
			int steps = 0;
			simplex simp = new simplex(f,start);
			acc = Pow(acc,simp.dim);
			do{
			double phiref = simp.ref_val();
			if(phiref < simp.minval){
				if(simp.exp_val() < phiref)simp.expansion();
				else simp.reflection();
			}
			else{
				if(phiref < simp.maxval) simp.reflection();
				else if(simp.con_val() < simp.maxval) simp.contraction();
				else simp.reduction();
			}
			steps++;
			}while(simp.volume()>acc);
			return(simp.min(),simp.minval,steps);
	}//downhill_sim
}//min
