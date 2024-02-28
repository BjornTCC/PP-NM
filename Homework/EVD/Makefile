CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: Out.txt Out.hydrogen.txt

Out.HydrogenRmax.svg: Out.HydrogenRmax.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "rmax";\
		set ylabel "Energy";\
		set title "Convergence of energy wrt. rmax";\
		plot "$<" with lp \
		'|tee log.gpi | gnuplot

Out.txt: main.exe
	mono $< > $@

Out.hydrogen.txt: hydrogen.exe Makefile
	mono $< -rmax:10 -dr:0.05 > $@

Out.HydrogenRmax.txt: hydrogen.exe Makefile
	>$@ 
	for R in $$(seq 7 1 14); do \
		mono $< -rmax:$$R -dr:0.05 1> out;\
	done

main.exe: main.cs mat.dll; $(MKEXE)

hydrogen.exe: hydrogen.cs mat.dll; $(MKEXE)

mat.dll: ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs jacobi.cs; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut* [Ll]og*
