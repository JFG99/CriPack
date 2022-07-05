Goals:

- Rewrite the bulk of the code to be clean, readable, easily debugged.
- Fix the patching bugs by deciphering the mystery meta data.  
- Clean up the DI.
- Potentionally rewrite the sources. File, Meta and Patch, at first seemed like the right source types but after getting into Patch I see that is wrong. 
	These should be in the base Repo and the sources should be the different types of archive structures. ie: Content, Toc, Etoc, etc.  
- Clean up naming and ensure conventions 


TODO:
- Utlize DI with the Interfaces in the repository to eliminate repeated code structures for different parsers and writers.
- DEFINETLY eliminate all the switch statements.  
- Row class is odd.  Lots of properties that are used independently for identification.  
	Should be able to refactor into a small number of interfaced properties that could 
	then be single source selected through a reflection like linq query. (Try to handle this without reflection) 


Current Work:
- (Complete) Initial parsing is finished. Moving on to the beginExtractCPK method from MainWindow. 
- Patching requires a lot of sight to various headers as a single change in byte length has to be propogated everywhere and must iterate through all files in the archive. 
- Decide if source refactoring is worth the effort now or after patching algorithm is figured out. 

