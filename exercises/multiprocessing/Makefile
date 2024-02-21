CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$nthreads %e %U"

Out.times.svg: out.times.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "Number of processors" ;\
		set ylabel "Time (s)" ;\
		set title "Harmonic sum with multiprocessing" ;\
		plot "$<" with lp ;\
		' | tee log.gpi | gnuplot

N = 1e10
out.times.txt: main.exe
	for nthreads in 1 2 3 4 5 6 7 8; do\
		$(TIME) mono main.exe -nthreads:$$nthreads -nterms:$(N); \
	done

main.exe: main.cs; $(MKEXE)

main-for.exe: main-for.cs; $(MKEXE)

clean:
	$(RM) *.exe *.dll [Oo]ut* [Ll]og [Ll]og*
