class main{
	public static void Main(){
		for(double x = -3; x <= 3; x+=1.0/8){
			System.Console.WriteLine($"{x} {sfuns.erf(x)}");
		}
	}//Main
}//main
