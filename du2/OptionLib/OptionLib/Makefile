ifndef TARGET
	TARGET=./bin/Debug
else
	TARGET=./bin/$(TARGET)
endif

MCS=mcs
ifndef (RELEASE)
	MCSFLAGS=-debug --stacktrace
endif

PATH=c:\Program Files (x86)\Mono-2.11\bin\

USECASES_EXE=$(TARGET)/usecases.exe
USECASES_PDB=$(TARGET)/optionlib.dll
USECASES_SRC=./UseCases

OPTIONLIB_DLL=$(TARGET)/optionlib.dll
OPTIONLIB_PDB=$(TARGET)/optionlib.dll
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
	./OptionLib/Options/OptionWithOptionableParameter.cs

$(OPTIONLIB_DLL): $(OPTIONLIB_SRC) 
	-mkdir -p $(TARGET)
	$(PATH)$(MCS) $(MCSFLAGS) $(LIBS) -target:library -out:$(OPTIONLIB_DLL) $(OPTIONLIB_SRC)

# common targets

all:	$(OPTIONLIB_DLL) $(USECASES_EXE)

clean:
	-rm -f "$(OPTIONLIB_DLL)" 2> /dev/null
	-rm -f "$(OPTIONLIB_PDB)" 2> /dev/null

# project names as targets

optionlib: $(OPTIONLIB_DLL) 
