CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$N %e %U"

all: Out.txt Out.HydrogenRmax.svg Out.HydrogenDr.svg Out.times.svg

Out.HydrogenRmax.svg: Out.HydrogenRmax.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "rmax";\
		set ylabel "Energy";\
		set title "Convergence of energy wrt. rmax, dr = 0.05";\
		plot "$<" using 1:3 with lp \
		'|tee log.gpi | gnuplot

Out.HydrogenDr.svg: Out.HydrogenDr.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "dr";\
                set ylabel "Energy";\
		set xrange [0.2:0.01] ;\
                set title "Convergence of energy wrt. dr rmax = 10";\
                plot "$<" using 2:3 with lp \
                '|tee log.gpi | gnuplot

Out.HydrogenWavef.svg: Out.HydrogenWavef.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "r";\
                set ylabel "Psi";\
                set xrange [0:20] ;\
                set title "Wavefunctions";\
                f1(x) = 2*x*exp(-x);\
		f2(x) = -x*(1-x/2)*exp(-x/2)/sqrt(2) ;\
		f3(x) = 2*x*(1-2*x/3 + 2*x*x/27)*exp(-x/3)/sqrt(27) ;\
		plot "$<" using 1:2 with p pt 3 ps 0.3 title "n = 1" \
		,"$<" using 1:3 with p pt 3 ps 0.3 title "n = 2" \
		,"$<" using 1:4 with p pt 3 ps 0.3 title "n = 3" \
		,f1(x) with lines lt rgb "red"  title "u1(r)"\
                ,f2(x) with lines lt rgb "brown" title "u2(r)"\
                ,f3(x) with lines lt rgb "grey0" title "u3(r)"\
                '|tee log.gpi | gnuplot

Out.times.svg: Out.times.txt Out.times_opt.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "Matrix size N";\
		set ylabel "Decomposition time";\
		set key left top ;\
		set title "Running time as function of matrix size";\
		f(x)=b+(x/a)**c;\
		b=1; a=90; c=3;\
                fo(x)=bo+(x/ao)**co;\
                bo=1; ao=90; co = 3;\
		fit f(x) "Out.times.txt" via a,b;\
		fit fo(x) "Out.times_opt.txt" via ao,bo;\
		plot "Out.times.txt" with lp title "Jacobi algorithm"\
		,"Out.times_opt.txt" with lp title "Optimized Jacobi algorithm" \
		,f(x) title sprintf("fit: (n/%.f)^{%.2f}+%.3f",a,c,b) \
                ,fo(x) title sprintf("fit: (n/%.f_opt)^{%.2f}+%.3f",ao,c,bo) \
		'|tee log.gpi |gnuplot

Out.txt: main.exe
	mono $< > $@

Out.times.txt: time.exe Makefile
	>$@
	for N in $$(seq 100 25 300); do \
		$(TIME) mono $< -msize:$$N 1>out 2>err ;\
	done

Out.times_opt.txt: time_opt.exe Makefile
	>$@
	for N in $$(seq 100 25 300); do \
		$(TIME) mono $< -msize:$$N 1>out 2>err ;\
	done

Out.HydrogenRmax.txt: hydrogen.exe Makefile
	>>$@ 
	for R in $$(seq 4 1 10); do \
		mono $< -rmax:$$R -dr:0.05 1>> $@;\
	done

Out.HydrogenDr.txt: hydrogen.exe Makefile
	>>$@
	for Dr in $$(seq 0.01 0.01 0.2); do \
                mono $< -rmax:10 -dr:$$Dr 1>> $@;\
        done

Out.HydrogenWavef.txt: hydrogen.exe Makefile
	mono $< -rmax:25 -dr:0.05 -Wavef_conv:Wavef > $@

main.exe: main.cs mat.dll; $(MKEXE)

hydrogen.exe: hydrogen.cs mat.dll; $(MKEXE)

time.exe: time.cs mat.dll; $(MKEXE)

time_opt.exe: time_opt.cs mat.dll; $(MKEXE)

mat.dll: ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs jacobi.cs; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut* [Ll]og*
