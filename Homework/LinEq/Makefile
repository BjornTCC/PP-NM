CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

Out.txt: main.exe
	mono $< > $@

main.exe: main.cs vector.dll ../../libs/matrix/matrix.dll QRGS.dll; $(MKEXE)

vector.dll: ../../libs/matrix/vector.cs; $(MKLIB)

matrix.dll: ../../libs/matrix/matrix.cs; $(MKLIB)

QRGS.dll: QRGS.cs matrix.dll; $(MKLIB)

clean: 
	$(RM) *.dll *.exe [Oo]ut*
