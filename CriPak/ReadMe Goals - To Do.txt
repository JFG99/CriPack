Goals:

Rewrite the bulk of the code to be clean, readable, easily debugged.
Fix the patching bug.

Nice to have:
Code to interfaces and then implement Depency Injection.





TODO:
Code is being implemented with interfaces in mind, but they are not built yet.  As things start to wrap up, 
	refactor concrete type to interfaces.

Utlize DI with the Interfaces in the repository to eliminate repeated code structures for different parsers and writers.

DEFINETLY eliminate all the switch statements.  
	
For the most part I did not spend much time with the column and row classes.  
	They are horribly used and really need some attention.

Row class is odd.  Lots of properties that are used independently for identification.  
	Should be able to refactor into a small number of interfaced properties that could 
	then be single source selected through a reflection like linq query.  




Current Work:
	Initial parsing is finished. Moving on to the beginExtractCPK method from MainWindow.  
