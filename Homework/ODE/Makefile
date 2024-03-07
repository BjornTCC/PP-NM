CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: OutA.svg OutB.txt OutB.svg OutC.svg

OutC.svg: DataC.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x";\
		set ylabel "y";\
		set size ratio -1;\
		set title "Figure-8 solution";\
		plot "$<" using 1:2 with lines notitle\
		, "$<" using 3:4 with lines notitle\
		, "$<" using 5:6 with lines notitle\
		'|tee logC.gpi | gnuplot


OutB.svg: DataBNC.txt DataBNE.txt DataBGR.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "x";\
                set ylabel "y";\
		set size ratio -1;\
		set grid;\
                set term svg size 1200, 600 ;\
		set multiplot layout 1,3 columns;\
                set title "Circular Newtonian orbit";\
		plot "DataBNC.txt" using (1/$$2)*cos($$1):(1/$$2)*sin($$1) with lines notitle;\
		set title "Elliptic Newtonian orbit";\
                plot "DataBNE.txt" using (1/$$2)*cos($$1):(1/$$2)*sin($$1) with lines notitle;\
		set title "Relativistic precession in GR";\
                plot "DataBGR.txt" using (1/$$2)*cos($$1):(1/$$2)*sin($$1) with lines notitle;\
		unset multiplot;\
                '|tee logB.gpi | gnuplot

OutA.svg: DataAHarm.txt DataAPend.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "t";\
		set grid;\
		set size 1,1;\
		set title "Solution to Harmonic oscillator and damped pendulum";\
		plot "DataAHarm.txt" using 1:2 with l t "Harmonic oscillator"\
		,"DataAPend.txt" using 1:2 with l t "theta(t)"\
		,"DataAPend.txt" using 1:3 with l t "omega(t)"\
		'|tee logA.gpi | gnuplot

DataAHarm.txt: mainA.exe
	mono $< 1> $@ 2> DataAPend.txt

DataBNC.txt: mainB.exe Makefile
	mono $< -u0:1 1> $@ 2>> OutB.txt

DataBNE.txt: mainB.exe Makefile
	mono $< -u0:1 -uprime0:-0.5 1> $@ 2>> OutB.txt

DataBGR.txt: mainB.exe Makefile
	mono $< -u0:1 -uprime0:-0.5 -epsilon:0.01 -phiL:20 1> $@ 2>> OutB.txt

OutB.txt: 
	echo 'Test of ODE solver with no path-recording:' > $@

DataC.txt: mainC.exe
	mono $< > $@

mainA.exe: mainA.cs genlib.dll ode.dll; $(MKEXE)

mainB.exe: mainB.cs genlib.dll ode.dll; $(MKEXE)

mainC.exe: mainC.cs genlib.dll ode.dll; $(MKEXE)

genlib.dll: ../../libs/matrix/vector.cs ../../libs/genlist/genlist.cs; $(MKLIB)

ode.dll: ode.cs genlib.dll; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* [Dd]ata* [Ll]og*
