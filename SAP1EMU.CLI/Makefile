.PHONY: publish zip
.DEFAULT_GOAL := all


CC = dotnet publish
CC_FLAGS = -c Release --framework netcoreapp3.1
CC_SELF =  --self-contained true
CC_NOSF = --self-contained false
PROJ = SAP1EMU.CLI.csproj
PROJ_NAME = SAP1Emu-CLI


PUB_DEST =  ./bin/Release/netcoreapp3.1/publish
ZIP_DEST = ./bin/Release/netcoreapp3.1/publish/output

tar_FLAGS = -cvf
zip_FLAGS = -r 

# Runtimes *********************
# No ending whitespaces allowed
WIN_RT = win10-x64
WIN_ARM_RT = win10-arm

OSX_RT = osx.10.14-x64

LINUX_RT = linux-x64
LINUX_ARM = linux-arm
LINUX_MUSL = linux-musl-x64
# ******************************

all: publish output_dir zip tar

publish:
	# Windows x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${WIN_RT} -o ${PUB_DEST}/${WIN_RT}
	
	# Windows ARM
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${WIN_ARM_RT} -o ${PUB_DEST}/${WIN_ARM_RT} 

	# OSX x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${OSX_RT} -o ${PUB_DEST}/${OSX_RT} 

	# Linux x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_RT} -o ${PUB_DEST}/${LINUX_RT} 

	# Linux ARM
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_ARM} -o ${PUB_DEST}/${LINUX_ARM} 

	# Linux MUSL -x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_MUSL} -o ${PUB_DEST}/${LINUX_MUSL} 


output_dir:
	if [ ! -d "${ZIP_DEST}" ]; then mkdir ${ZIP_DEST}; fi; 
	
zip:
	# Windows x64
	zip ${zip_FLAGS} ${ZIP_DEST}/${PROJ_NAME}-${WIN_RT}.zip ${PUB_DEST}/${WIN_RT} -j

	# Windows ARM
	zip ${zip_FLAGS} ${ZIP_DEST}/${PROJ_NAME}-${WIN_ARM_RT}.zip ${PUB_DEST}/${WIN_ARM_RT} -j

tar:
	# OSX x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${PROJ_NAME}-${OSX_RT}.tar -C ${PUB_DEST} ${OSX_RT}

	# Linux x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${PROJ_NAME}-${LINUX_RT}.tar -C ${PUB_DEST} ${LINUX_RT}

	# Linux ARM
	tar ${tar_FLAGS}  ${ZIP_DEST}/${PROJ_NAME}-${LINUX_ARM}.tar -C ${PUB_DEST} ${LINUX_ARM}

	# Linux MUS -x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${PROJ_NAME}-${LINUX_MUSL}.tar -C ${PUB_DEST} ${LINUX_MUSL}
