using static System.Console;

class time{
	public static void Main(string[] args){
		int N = 1;
		foreach(string arg in args){
			var words = arg.Split(':');
			if(words[0] == "-msize")N= int.Parse(words[1]);
		}
		System.Random random = new System.Random();
		matrix A = new matrix(N);
		for(int i = 0; i < N; i++)
		for(int j = 0; j < N; j++){ 
			A[i,j] = random.NextDouble();
			A[j,i] = A[j,i];
		}
		jacobi.cyclic_opt(A);
	}//Main
}//time
