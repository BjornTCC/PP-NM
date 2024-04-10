CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

grad_centraltest.exe: matlib.dll grad_centraltest.cs; $(MKEXE)

gradtest.exe: matlib.dll gradtest.cs; $(MKEXE)

matlib.dll: ../libs/matrix/matrix.cs ../libs/matrix/vector.cs; $(MKLIB)
