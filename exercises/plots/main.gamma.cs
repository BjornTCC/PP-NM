class main{
        public static void Main(){
                for(double x = -3; x <= 5; x+=1.0/32){
                        System.Console.WriteLine($"{x} {sfuns.gamma(x)}");
                }
		int j = 1;
		for(int i = 1; i <= 5; i++){
		System.Console.Error.WriteLine($"{i} {j}");
		j*=i;
		}
        }//Main
}//main	
