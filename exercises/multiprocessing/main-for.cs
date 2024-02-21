using System.Linq;

public class main{
	public static void Main(string[] args){
                int nterms = (int)1e7;
                foreach(string arg in args){
                        var words = arg.Split(':');
                        if(words[0] == "-nterms")nterms = (int)double.Parse(words[1]);
			}
		System.Console.WriteLine($"Main: nterms = {nterms}");
		var total = new System.Threading.ThreadLocal<double>( ()=>0, trackAllValues:true);
		System.Threading.Tasks.Parallel.For( 1, nterms+1, (int i)=>total.Value+=1.0/i );
		double totalsum=total.Values.Sum();
		System.Console.WriteLine($"Main: sum = {totalsum}");
	}//Main
}//main
