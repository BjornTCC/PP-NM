CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$N %e %U"

extlib.dll: ../../libs/matrix/vector.cs ../Week19_Minimization/min.cs\
	../../libs/matrix/matrix.cs ../../libs/QRDecomp/QRGS.cs; $(MKLIB)

ann.dll: ann.cs extlib.dll; $(MKLIB)
