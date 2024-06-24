CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

default: Out_convolution.svg Test_Result.txt

Out_convolution.svg: data.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "s";\
		set ylabel "F(s)*G(s)";\
		set size 1,1;\
		plot "$<" using 1:2 with l title "analytic"\
		, "$<" using 1:3 with l title "numerical"\
		'|tee log.gpi|gnuplot

Test_Result.txt: test.exe;
	mono $< 1> $@ 2> err1.txt

data.txt: conv.exe;
	mono $< 1> $@ 2> err2.txt

conv.exe: int2.dll matlib.dll conv.cs; $(MKEXE)

test.exe: int2.dll matlib.dll test.cs; $(MKEXE)

int2.dll: int2.cs matlib.dll; $(MKLIB)

matlib.dll: ../libs/Integration/int.cs ../libs/matrix/vector.cs ../libs/matrix/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* [Ll]og* *.[Ll]og err*
