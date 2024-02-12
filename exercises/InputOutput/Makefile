default: Out.txt Out2.txt Out3.txt Makefile

Out.txt: main.exe 
	mono $< -numbers:1,-2,-3,4,5 1> $@ 

Out2.txt: main2.exe
	mono $< < input.txt 2> $@

Out3.txt: main3.exe input3.txt 
	mono $< -input:input3.txt -output:Out3.txt

main.exe: main.cs
	mcs -target:exe -out:$@ $<

main2.exe: main2.cs
	mcs -target:exe -out:$@ $<

main3.exe: main3.cs
	mcs -target:exe -out:$@ $<

clean:
	$(RM) Out.txt Out2.txt Out3.txt main.exe main2.exe main3.exe

