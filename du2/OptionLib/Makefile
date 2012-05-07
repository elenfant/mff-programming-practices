ifndef TARGET
	TARGET=./bin/Debug
else
	TARGET=./bin/$(TARGET)
endif

MCS=mcs
ifndef (RELEASE)
	MCSFLAGS=-debug --stacktrace
endif


USECASES_EXE=UseCases/$(TARGET)/usecases.exe
USECASES_PDB=UseCases/$(TARGET)/optionlib.dll
USECASES_SRC=./UseCases

OPTIONLIB_DLL=OptionLib/$(TARGET)/optionlib.dll
OPTIONLIB_PDB=OptionLib/$(TARGET)/optionlib.dll
OPTIONLIB_SRC=./OptionLib/ArgumentParser.cs \
	./OptionLib/Printer.cs \
	./OptionLib/ProgramOptionsBase.cs \
	./OptionLib/ProgramOption.cs \
	./OptionLib/Options/BoundsAttribute.cs \
	./OptionLib/Options/LongName.cs \
	./OptionLib/Options/ShortName.cs \
	./OptionLib/Options/NameAttribute.cs \
	./OptionLib/Options/Option.cs \
	./OptionLib/Options/OptionBase.cs \
	./OptionLib/Options/OptionWithParameter.cs \
	./OptionLib/Options/OptionWithOptionableParameter.cs \
	./OptionLib/OptionLibExceptions.cs

$(OPTIONLIB_DLL): $(OPTIONLIB_SRC) 
	-mkdir -p UseCases/$(TARGET)
	-mkdir -p OptionLib/$(TARGET)
	$(MCS) $(MCSFLAGS) $(LIBS) -target:library -out:$(OPTIONLIB_DLL) $(OPTIONLIB_SRC)

# common targets

all:	$(OPTIONLIB_DLL) $(USECASES_EXE)

clean:
	-rm -f "$(OPTIONLIB_DLL)" 2> /dev/null
	-rm -f "$(OPTIONLIB_PDB)" 2> /dev/null

# project names as targets

optionlib: $(OPTIONLIB_DLL) 
