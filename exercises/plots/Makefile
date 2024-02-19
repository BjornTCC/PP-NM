CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

default: Out.Erf.gnuplot.svg Out.Gamma.gnuplot.svg Out.Lngamma.gnuplot.svg

Out.Erf.gnuplot.svg: Out.erf.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x";\
		set ylabel "erf(x)";\
		set title "Error function";\
		set grid ;\
		plot [-3:3]\
			"$<" with lines notitle \
			,"Erfdata.txt" using 1:2 with points pointtype 6 notitle\
			,"Erfdata.txt" using (-($$1)):(-($$2)) with points pointtype 6 notitle ;\
	'| tee log.gpi | gnuplot 

Out.Gamma.gnuplot.svg: Out.Gamma.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
		set yr [-5:10] ;\
                set xlabel "x";\
                set ylabel "gamma(x)";\
                set title "Gamma function";\
                set grid ;\
		set xzeroaxis linetype -1 linewidth 2.5;\
		set yzeroaxis linetype -1 linewidth 2.5;\
                plot [-3:5]\
                        "$<" with lines notitle \
			,"GammaData.txt" using 1:2 with points pointtype 6 notitle ;\
        '| tee log.gpi | gnuplot

Out.Lngamma.gnuplot.svg: Out.Lngamma.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "x";\
                set ylabel "lngamma(x)";\
                set title "Log of gamma function";\
                set grid ;\
                plot [0:10]\
                        "$<" with lines notitle \
                        ,"LngammaData.txt" using 1:2 with points pointtype 6 notitle ;\
        '| tee log.gpi | gnuplot

Out.erf.txt: main.erf.exe
	mono $< > $@

Out.Gamma.txt: main.gamma.exe
	mono $< 1> $@ 2> GammaData.txt

Out.Lngamma.txt: main.lngamma.exe
	mono $< 1> $@ 2> LngammaData.txt

sfuns.dll: sfuns.cs ; $(MKLIB)

main.erf.exe : main.erf.cs sfuns.dll; $(MKEXE)

main.gamma.exe: main.gamma.cs sfuns.dll; $(MKEXE)

main.lngamma.exe: main.lngamma.cs sfuns.dll; $(MKEXE)

clean:
	$(RM) *.exe *.dll [Oo]ut*
