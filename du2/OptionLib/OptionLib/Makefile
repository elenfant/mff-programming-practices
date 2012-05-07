ifndef TARGET
	TARGET=./bin/Debug
else
	TARGET=./bin/$(TARGET)
endif

MCS=gmcs
ifndef (RELEASE)
	MCSFLAGS=-debug --stacktrace
endif

LIBS=


OPTIONLIB_DLL=$(TARGET)/optionlib.dll
OPTIONLIB_PDB=$(TARGET)/optionlib.dll
OPTIONLIB_SRC=././ArgumentParser.cs \
	././Printer.cs \
	././ProgramOptionsBase.cs \
	././ProgramOption.cs \
	././Options/BoundsAttribute.cs \
	././Options/LongName.cs \
	././Options/ShortName.cs \
	././Options/NameAttribute.cs \
	././Options/Option.cs \
	././Options/OptionBase.cs \
	././Options/OptionWithParameter.cs \
	././Options/OptionWithOptionableParameter.cs

$(OPTIONLIB_DLL): $(OPTIONLIB_SRC) 
	-mkdir -p $(TARGET)
	$(MCS) $(MCSFLAGS) $(LIBS) -target:library -out:$(OPTIONLIB_DLL) $(OPTIONLIB_SRC)

# common targets

all:	$(OPTIONLIB_DLL)

clean:
	-rm -f "$(OPTIONLIB_DLL)" 2> /dev/null
	-rm -f "$(OPTIONLIB_PDB)" 2> /dev/null

# project names as targets

optionlib: $(OPTIONLIB_DLL) 
