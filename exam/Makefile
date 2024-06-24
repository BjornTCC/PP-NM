CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

test_result.txt: test.exe;
	mono $< > $@

test.exe: int2.dll matlib.dll test.cs; $(MKEXE)

int2.dll: int2.cs matlib.dll; $(MKLIB)

matlib.dll: ../libs/Integration/int.cs ../libs/matrix/vector.cs ../libs/matrix/matrix.cs; $(MKLIB)
