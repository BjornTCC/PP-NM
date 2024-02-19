class main{
        public static void Main(){
                for(double x = 1.0/16; x <= 10; x+=1.0/16){
                        System.Console.WriteLine($"{x} {sfuns.lngamma(x)}");
                }
		int j = 1;
		for(int i = 1; i <= 10; i++){
			System.Console.Error.WriteLine($"{i}, {System.Math.Log(j)}");
			j*=i;
		}
        }//Main
}//main
