using System;
using static System.Math;

public class ann{
	public readonly int n; /* number of neurons */
	public int steps;
	Func<double,double> f = x => x*Exp(-x*x);	/* activation function */
	Func<double,double> df = x => (1-2*x*x)*Exp(-x*x); /* derivative of function */
	Func<double,double> If = x => -Exp(-x*x)/2;	/* antiderivative of function */
	public vector p; /* network parameters */
	
	//constructors
	public ann(int m){
		n = m; p = new vector(3*n);
		for(int i=0;i<n;i++){p[3*i]=0;p[3*i+1]=1; p[3*i+2]=1;}
	}

	public ann(int m, Func<double,double> g){ /* note no derivative response in this case */
		n = m; p = new vector(n,3); f=g;
                for(int i=0;i<n;i++){p[3*i]=0;p[3*i+1]=1; p[3*i+2]=1;}
	}
	
	//Response
	double response(double x,vector v){
		double res = 0;
		for(int i=0;i<n;i++)res+=v[3*i+2]*f((x-v[3*i])/v[3*i+1]);
		return res;
	}

	public double response(double x){return response(x,p);}
	
	public double dresponse(double x){
		double res = 0;
		for(int i=0;i<n;i++)res+=p[3*i+2]*df((x-p[3*i])/p[3*i+1])/p[3*i+1];
		return res;
	}//dresponse
	
	public double Iresponse(double a,double b){
		double res = 0;
		for(int i=0;i<n;i++)res+=p[3*i+2]*p[3*i+1]*(If((b-p[3*i])/p[3*i+1]) - If((a-p[3*i])/p[3*i+1]));
		return res;
	}//Iresponse
	//train
	public void train_interp(vector x,vector y){
		Func<vector,double> cost = delegate(vector v){double cst = 0; 
			for(int i=0;i<x.size;i++)cst+=Pow(response(x[i],v)-y[i],2); return cst;};
		var minip = min.downhill_sim(cost,p);
		steps = minip.Item3;	
		p = minip.Item1;
	}

}//ann

public class diff_nn{
	public readonly int n; /* number of hidden nodes */
	public int steps;
        Func<double,double> f = x => x*Exp(-x*x);       /* activation function */
        Func<double,double> df = x => (1-2*x*x)*Exp(-x*x); /* derivative of function */
	Func<double,double> ddf= x => -2*x*(3-2*x*x)*Exp(-x*x); /* double derivative */
        Func<double,double> If = x => -Exp(-x*x)/2;     /* antiderivative of function */
        public vector p; /* network parameters */

	//constructors
        public diff_nn(int m){
                n = m; p = new vector(3*n);
                for(int i=0;i<n;i++){p[3*i]=0;p[3*i+1]=1; p[3*i+2]=1;}
        }

        //Response
        double response(double x,vector v){
                double res = 0;
                for(int i=0;i<n;i++)res+=v[3*i+2]*f((x-v[3*i])/v[3*i+1]);
                return res;
        }

        public double response(double x){return response(x,p);}
	
	public double dresponse(double x){return dresponse(x,p);}

	public double ddresponse(double x){return ddresponse(x,p);}

        double dresponse(double x, vector v){
                double res = 0;
                for(int i=0;i<n;i++)res+=v[3*i+2]*df((x-v[3*i])/v[3*i+1])/v[3*i+1];
                return res;
        }//dresponse
	
	double ddresponse(double x, vector v){
                double res = 0;
                for(int i=0;i<n;i++)res+=v[3*i+2]*ddf((x-v[3*i])/v[3*i+1])/Pow(v[3*i+1],2);
                return res;
        }//ddresponse
	
        public double Iresponse(double a,double b){
                double res = 0;
                for(int i=0;i<n;i++)res+=p[3*i+2]*p[3*i+1]*(If((b-p[3*i])/p[3*i+1]) - If((a-p[3*i])/p[3*i+1]));
                return res;
        }//Iresponse
	
	public void train(
			Func<Func<double,double>[],double,double> Phi, /*diff equation = 0 */
			double a, double b,						/* interval */
			double c, 							/*starting point*/
			double[] Fc,							/* initial values */
			double alp=1, 
			double bet=1){
		Func<vector,double> cost = delegate(vector I){
			double res = alp*Pow(Fc[0]-response(c,I),2) + bet*Pow(Fc[1]-dresponse(c,I),2);
			Func<double,double>[] y = {x=>response(x,I),x=>dresponse(x,I),x=>ddresponse(x,I)};
			Func<double,double> Phi2 = x=>Pow(Phi(y,x),2);
			res += integrate.adint(Phi2,a,b).Item1;
			return res;
		};
		var minip = min.downhill_sim(cost,p);
                steps = minip.Item3;
                p = minip.Item1;
	}//train	
		
}//diff_nn
