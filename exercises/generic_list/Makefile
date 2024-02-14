Out.txt: main.exe Makefile input.txt
	mono $< <input.txt 1> $@ 2> log

main.exe: main.cs genlist.dll
	mcs main.cs -out:$@ -target:exe -reference:genlist.dll

genlist.dll: genlist.cs
	mcs genlist.cs -out:$@ -target:library

clean:
	$(RM) *.dll *.exe log*
