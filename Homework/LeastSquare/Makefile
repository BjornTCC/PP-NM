CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$N %e %U"

all: Out.txt Out.fit.svg

Out.fit.svg: data.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@";\
		set xlabel "t (days)" ;\
		set ylabel "Activity" ;\
		set title "exponential fit of radioactive decay of 224Ra";\
		f(x) = a*exp(-b*x) ;\
		fp(x) = a*exp(da)*exp(-(b-db)*x) ;\
		fm(x) = a*exp(-da)*exp(-(b+db)*x) ;\
		a = 142.403540630783; b = 0.171; da = 0.0354019292641024; db = 0.0071566223420022;\
		plot "$<" with errorbars title "data"\
		,f(x) title sprintf("fit: %.f*e^{-%.2f t}",a,b)\
		,fp(x) with lines title sprintf("fit with sigma added")\
		,fm(x) with lines title sprintf("fit with sigma subtracted")\
		'|tee log.gpi | gnuplot

Out.txt: main.exe
	mono $< > $@

main.exe: main.cs matlib.dll; $(MKEXE)

matlib.dll: ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs ../../libs/QRDecomp/QRGS.cs lsfit.cs; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut* [Ll]og*
