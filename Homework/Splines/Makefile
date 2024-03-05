CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: Out.Linterp.svg Out.qterp.svg Out1.txt Outq1.txt Outq2.txt

Out.Linterp.svg: Out1.txt Out2.txt Makefile
	echo '\
		set terminal svg background "white" ;\
		set out "$@" ;\
		set xlabel "x";\
		set ylabel "y";\
                set multiplot layout 2,1 columns ;\
		set title "Linear interpolation";\
		plot "Out2.txt" with p pt 7 ps 0.5 title "Data"\
		,"Out1.txt" using 1:2 with l notitle;\
		set title "Integral of Linear interpolation";\
		plot "Out1.txt" using 1:3 with l notitle;\
		unset multiplot;\
		'|tee log.gpi | gnuplot

Out.qterp.svg: Outq1.txt Outq2.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "x";\
                set ylabel "y";\
		set size ratio 1.5;\
		set multiplot layout 1,3 columns scale 1.1;\
                set title "Quadratic spline";\
		plot "Outq2.txt" using 1:2 with p pt 7 ps 0.5 title "Data"\
		,"Outq2.txt" using 1:3 with p pt 7 ps 0.5 notitle \
                ,"Outq2.txt" using 1:4 with p pt 7 ps 0.5 notitle \
		,"Outq1.txt" using 1:2 with l notitle linecolor rgb "#4627E2"\
                ,"Outq1.txt" using 1:3 with l notitle linecolor rgb "#27E23A"\
                ,"Outq1.txt" using 1:4 with l notitle linecolor rgb "#F0860C";\
		set title "Quadratic spline derivatives";\
                plot "Outq1.txt" using 1:5 with l notitle linecolor rgb "#4627E2"\
                ,"Outq1.txt" using 1:6 with l notitle linecolor rgb "#27E23A"\
                ,"Outq1.txt" using 1:7 with l notitle linecolor rgb "#F0860C";\
		set title "Quadratic spline integrals";\
		plot "Outq1.txt" using 1:8 with l notitle linecolor rgb "#4627E2"\
                ,"Outq1.txt" using 1:9 with l notitle linecolor rgb "#27E23A"\
		,"Outq1.txt" using 1:10 with l notitle linecolor rgb "#F0860C";;\
		unset multiplot;\
                '|tee log.gpi | gnuplot

Out1.txt: main.exe
	mono $< 1> $@ 2> Out2.txt

Outq1.txt: mainq.exe
	mono $< -output:Outq2.txt 2> $@ 1> OutqTest.txt

Outq2.txt: Makefile main.exe

mainq.exe: mainq.cs spline.dll; $(MKEXE)

main.exe: main.cs spline.dll; $(MKEXE)

spline.dll: spline.cs ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs ../../libs/QRDecomp/QRGS.cs; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut*
