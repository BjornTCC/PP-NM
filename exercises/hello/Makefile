Out.txt : hello.exe
	mono hello.exe > Out.txt

hello.exe : hello.cs
	mcs hello.cs

.PHONEY : clean
clean:
	rm -f hello.exe main Out.txt
