using System;
using static System.Console;

static class main{
	
public static int Main(){
        var list = new genlist<double[]>(); 
	char[] delimiters = {' ','\t'};
	var options = StringSplitOptions.RemoveEmptyEntries;
	for(string line = ReadLine(); line!=null; line = ReadLine()){
		var words = line.Split(delimiters,options);
		int n = words.Length;
		var numbers = new double[n];
		for(int i=0;i<n;i++) numbers[i] = double.Parse(words[i]);
		list.add(numbers);
       		}
	for(int i=0;i<list.size;i++){
		var numbers = list[i];
		foreach(var number in numbers)Write($"{number : 0.00e+00;-0.00e+00} ");
		WriteLine();
        	}
	//Test of add and remove
	var RList = new genlist<double>();
	RList.add(1.0);
	RList.add(2.0);
	RList.add(3.0);
	RList.add(4.0);
	for(int i = 0; i < RList.size; i++)Write($"{RList[i]},");
	WriteLine();
	RList.remove(1);
	RList.remove(4);
	for(int i = 0; i < RList.size; i++)Write($"{RList[i]},");
	WriteLine();
	//Test of clist
	clist<int> a = new clist<int>();
                a.add(1);
                a.add(2);
                a.add(3);
                for( a.start(); a.current != null; a.next()){
                        WriteLine(a.current.item);
			}
	return 0;
        }
}
