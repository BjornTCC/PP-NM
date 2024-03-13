CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$N %e %U"

all: OutAB.svg outdataAB.txt outdataC.txt OutC.svg

OutC.svg: outdataC.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x";\
		set ylabel "y";\
		set grid;\
		f(x) = cos(x);\
		set title "Harmonic oscillator with Neural Network";\
		plot "$<" with l t "Network solution, 10 nodes" \
		,f(x) with l lt 0 t "exact solution"\
		'|tee logC.gpi | gnuplot

OutAB.svg: outdataAB.txt dataTable.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "x";\
                set ylabel "g";\
		set key outside;\
		g(x) = cos(5*x-1)*exp(-x*x);\
		set term svg size 700, 1000 ;\
		set multiplot layout 3,1 columns;\
                set title "Neural Network for interpolation";\
                set grid;\
                plot[-1.1:1.1] g(x) with l lt 0 t "g(x)"\
		,"dataTable.txt" with p pt 6 ps 0.5 t "Training data"\
		,"$<" using 1:2 with l t "3 nodes" linecolor rgb "#4627E2"\
		,"$<" using 1:5 with l t "5 nodes" linecolor rgb "#27E23A"\
		,"$<" using 1:8 with l t "9 nodes" linecolor rgb "#F0860C";\
		set title "derivative:";\
		plot[-1.1:1.1]"$<" using 1:3 with l t "3 nodes" linecolor rgb "#4627E2"\
                ,"$<" using 1:6 with l t "5 nodes" linecolor rgb "#27E23A"\
                ,"$<" using 1:9 with l t "9 nodes" linecolor rgb "#F0860C";\
		set title "integral from x=-1";\
		plot[-1.1:1.1]"$<" using 1:4 with l t "3 nodes" linecolor rgb "#4627E2"\
                ,"$<" using 1:7 with l t "5 nodes" linecolor rgb "#27E23A"\
                ,"$<" using 1:10 with l t "9 nodes" linecolor rgb "#F0860C";\
		unset multiplot;\
                '|tee logAB.gpi | gnuplot

outdataAB.txt: mainAB.exe
	mono $< 1> $@ 2> dataTable.txt

outdataC.txt: mainC.exe
	mono $< > $@

mainAB.exe: mainAB.cs ann.dll extlib.dll; $(MKEXE)

mainC.exe: mainC.cs ann.dll extlib.dll; $(MKEXE)

extlib.dll: ../../libs/matrix/vector.cs ../../libs/Minimization/min.cs\
	../../libs/matrix/matrix.cs ../../libs/QRDecomp/QRGS.cs\
	../../libs/Integration/int.cs; $(MKLIB)

ann.dll: ann.cs extlib.dll; $(MKLIB)

clean:
	$(RM) *.exe *.dll [Oo]ut* [Llog]* 
