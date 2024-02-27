CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$N %e %U"

default: Out.txt Out.times.svg Makefile

Out.times.svg: Out.times.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set logscale y 2 ;\
		set xlabel "Matrix size N";\
		set ylabel "real time";\
		set title "Running time as function of matrix size";\
		plot "$<" with lp ;\
		'|tee log.gpi |gnuplot

Out.times.txt: time.exe Makefile
	>$@
	for N in $$(seq 200 25 800); do \
		$(TIME) mono $< -msize:$$N 1>out 2>err ;\
	done

Out.txt: main.exe
	mono $< > $@

time.exe: time.cs matrix.dll QRGS.dll; $(MKEXE)

main.exe: main.cs matrix.dll QRGS.dll; $(MKEXE)

matrix.dll: ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs; $(MKLIB)

QRGS.dll: QRGS.cs matrix.dll; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut* [Ll]og*
