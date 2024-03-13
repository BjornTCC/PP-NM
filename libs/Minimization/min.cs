using System;
using static System.Math;

public class min{
	
	static vector grad(Func<vector,double> f, vector x){
		double eps = Pow(2,-26);
		int n = x.size;
		vector res = new vector(n), y = x.copy();
		for(int i=0;i<n;i++){
			double dx = Abs(x[i])*eps;
			y[i]+=dx;
			res[i] = (f(y)-f(x))/dx;
			y[i]=x[i];}
		return res;
	}//grad

	public static (vector,double,int) qnewton( //returns (vmin, f(vmin), #steps taken)
	Func<vector,double> f, /* objective function */
	vector start, /* starting point */
	double acc = 0.001 /* accuracy goal, on exit |gradient| should be < acc */
	){
	int n = start.size, step = 0;
	matrix B = matrix.id(n);
	vector gradf = new vector(n), Dx = new vector(n), x = start.copy();
	double lambda = 1, fx = 0, gamma = 0, sy = 0;
	vector y = new vector(n), u = new vector(n), a = new vector(n); //parameters for update
	do{
	step++;
	fx = f(x);
	gradf = grad(f,x);
	Dx = -B*gradf;
	lambda = 1;
	while(true){
		if(f(x+lambda*Dx)<fx){
			x+=lambda*Dx; 			//accept step
			y = grad(f,x+lambda*Dx)-gradf;	//update B using symmetric Broydens update
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
		if(lambda*1024 < 1){
			x+=lambda*Dx;
			B = matrix.id(n);
			break;}
		}
	}while(gradf.norm()>acc);
	return (x,fx,step);
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
				if(phi(points[i])<phi(points[nmax])) nmin = i;}
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
	
	public static (vector,double) downhill_sim(
			Func<vector,double> f, /* objective function */
			vector start, /* starting point */
			double acc = 0.001
			){
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
			}while(simp.volume()>acc);
			return(simp.min(),simp.minval);
	}//downhill_sim
}//min
