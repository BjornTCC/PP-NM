CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: OutA.txt OutBhiggs.svg outBdatahiggs.txt OutC.txt 

OutBhiggs.svg: datahiggs.txt outBdatahiggs.txt Makefile
	echo '\
                set terminal svg background "white" ;\
                set out "$@" ;\
                set xlabel "E [GeV]";\
                set ylabel "{/symbol s} (background subtracted)";\
		set title "Higgs particle resonance fit with newton method";\
		set grid;\
                plot "$<" with errorbars t "Data"\
		, "outBdatahiggs.txt" with l t "Breit-Wigner fit"\
                '|tee logB.gpi | gnuplot

OutA.txt: mainA.exe
	mono $< > $@

outBdatahiggs.txt: mainB.exe
	mono $< < datahiggs.txt 1> $@ 2> logBhiggs.txt

OutC.txt: mainC.exe
	mono $< > $@

mainA.exe: mainA.cs extlib.dll min.dll; $(MKEXE)

mainB.exe: mainB.cs extlib.dll min.dll; $(MKEXE)

mainC.exe: mainC.cs extlib.dll min.dll; $(MKEXE)

extlib.dll: ../../libs/matrix/matrix.cs ../../libs/matrix/vector.cs \
	../../libs/genlist/genlist.cs ../../libs/QRDecomp/QRGS.cs; $(MKLIB)

min.dll: min.cs extlib.dll; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* [Ll]og*
