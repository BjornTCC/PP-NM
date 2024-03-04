CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

Out.Linterp.svg: Out1.txt Out2.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x";\
		set ylabel "y";\
		set title "Linear interpolation";\
		plot "Out2.txt" with p pt 7 ps 0.5 title "Data"\
		,"Out1.txt" with l title "Interpolation"\
		'|tee log.gpi | gnuplot

Out1.txt: main.exe
	mono $< 1> $@ 2> Out2.txt

Outq1.txt: mainq.exe
	mono $< 1> $@ 2> Outq2.txt

mainq.exe: mainq.cs spline.dll; $(MKEXE)

main.exe: main.cs spline.dll; $(MKEXE)

spline.dll: spline.cs ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs; $(MKLIB)
