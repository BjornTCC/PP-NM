using System;
using static System.Console;
using static System.Math;

public static class hydrogen{
	public static int Main(string[] args){
		double rmax = 10, dr = 0.1;
		int EnergyLevel = 0;
		string Wavef_conv = "conv";
		foreach(string arg in args){//parse inputs
			var words = arg.Split(':');
			if(words[0] == "-rmax")rmax= double.Parse(words[1]);
			if(words[0] == "-dr")dr = double.Parse(words[1]);
			if(words[0] == "-EnergyLevel") EnergyLevel = int.Parse(words[1]);
			if(words[0] == "-Wavef_conv") Wavef_conv = words[1];
		}
		int N = (int)Floor(rmax/dr) - 1;
		vector KD = new vector(N), KOD = new vector(N-1), rs = new vector(N), Vs = new vector(N);
		for(int i = 0; i < N; i++){ //constructing matrices
			KD[i] = -2;
			rs[i] = (i+1)*dr;
			Vs[i] = -1.0/rs[i];
			if(i != N-1)KOD[i] = 1;
		}
		matrix K = -0.5*(matrix.diag(KD) + matrix.diag(KOD, 1) + matrix.diag(KOD, -1))*Pow(dr,-2);
		matrix V = matrix.diag(Vs);
		matrix H = K + V; //Hamiltonian
		(vector Energies, matrix EigenVectors) = jacobi.cyclic_opt(H);
		for(int i=0;i<3;i++){if(EigenVectors[i][1] < 0) EigenVectors[i]*=-1;}
	      	if(Wavef_conv == "conv")WriteLine($"{rmax} {dr} {Energies[EnergyLevel]}");
		if(Wavef_conv == "Wavef"){
		for(int i = 0; i < N; i++){
		WriteLine($"{rs[i]} {EigenVectors[0][i]/Sqrt(dr)} {EigenVectors[1][i]/Sqrt(dr)} {EigenVectors[2][i]/Sqrt(dr)}");}}
		return 0;	
	}//Main
}//main
