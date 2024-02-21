class main{

	class harmdata{ public long a,b; public double sum; }

	static void harm(object obj){
		harmdata d = (harmdata)obj;
		d.sum = 0;
		System.Console.WriteLine($"harmfunc: a = {d.a}, b = {d.b}");
		for(long i =d.a; i <= d.b; i++)d.sum += 1.0/i;
		System.Console.WriteLine($"harmfunc: sum = {d.sum}");
	}//harm

	public static void Main(string[] args){
		long nterms = (long)1e7, nthreads =1;
		foreach(string arg in args){
			var words = arg.Split(':');
			if(words[0] == "-nterms")nterms = (long)double.Parse(words[1]);
			if(words[0] == "-nthreads")nthreads = (long)double.Parse(words[1]);
			}
		System.Console.WriteLine($"Main: nterms = {nterms}, nthreads = {nthreads}");
		harmdata[] data = new harmdata[nthreads];
		long chunk = nterms/nthreads;
		for(int i=0; i < nthreads; i++){
			data[i] = new harmdata();
			data[i].a = i*chunk + 1;
			data[i].b = data[i].a + chunk;
			}
		data[nthreads - 1].b = nterms;
		var threads = new System.Threading.Thread[nthreads];
		System.Console.WriteLine($"Main: starting threads...");
		for(long i = 0; i<nthreads; i++){
			threads[i] = new System.Threading.Thread(harm);
			threads[i].Start(data[i]);
			}
		System.Console.WriteLine($"Main: waiting for threads to finish...");
		foreach(var thread in threads)thread.Join();
		double total = 0;
		foreach(harmdata datum in data)total += datum.sum;
		System.Console.WriteLine($"Main: total sum = {total}");
	}//Main
}//main
