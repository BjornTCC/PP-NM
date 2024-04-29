CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^)) # macros
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

gmin.exe: matlib.dll gmin.cs; $(MKEXE)

BBPSO.exe: matlib.dll BBPSO.cs; $(MKEXE)

QR_evd.exe: matlib.dll QR_evd.cs; $(MKEXE)

lanczos.exe: matlib.dll lanczos.cs; $(MKEXE)

SVD.exe: matlib.dll SVD.cs; $(MKEXE)

LU.exe: matlib.dll LU.cs; $(MKEXE)

cholesky.exe: matlib.dll cholesky.cs; $(MKEXE)

grad_centraltest.exe: matlib.dll grad_centraltest.cs; $(MKEXE)

gradtest.exe: matlib.dll gradtest.cs; $(MKEXE)

matlib.dll: ../libs/matrix/matrix.cs ../libs/matrix/vector.cs \
	../libs/LinEq/QRGS.cs ../libs/Minimization/min.cs; $(MKLIB)

clean:
	$(RM) *.exe *.dll
