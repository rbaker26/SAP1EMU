.PHONY: publish zip
.DEFAULT_GOAL := all


CC = dotnet publish
CC_FLAGS = -c Release --framework netcoreapp3.1
CC_SELF =  --self-contained true
CC_NOSF = --self-contained false
PROJ = SAP1EMU.Engine-CLI/SAP1EMU.Engine-CLI.csproj


PUB_DEST = SAP1EMU.Engine-CLI/bin/Release/netcoreapp3.1/publish
ZIP_DEST = SAP1EMU.Engine-CLI/bin/Release/netcoreapp3.1/publish/output

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
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${WIN_RT} -o ${PUB_DEST}/${WIN_RT}-self-contained --no-build
	
	# Windows ARM
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${WIN_ARM_RT} -o ${PUB_DEST}/${WIN_ARM_RT} 
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${WIN_ARM_RT} -o ${PUB_DEST}/${WIN_ARM_RT}-self-contained --no-build

	# OSX x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${OSX_RT} -o ${PUB_DEST}/${OSX_RT} 
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${OSX_RT} -o ${PUB_DEST}/${OSX_RT}-self-contained --no-build

	# Linux x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_RT} -o ${PUB_DEST}/${LINUX_RT} 
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${LINUX_RT} -o ${PUB_DEST}/${LINUX_RT}-self-contained --no-build

	# Linux ARM
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_ARM} -o ${PUB_DEST}/${LINUX_ARM} 
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${LINUX_ARM} -o ${PUB_DEST}/${LINUX_ARM}-self-contained --no-build

	# Linux MUS -x64
	${CC} ${PROJ} ${CC_FLAGS} ${CC_NOSF} --runtime ${LINUX_MUSL} -o ${PUB_DEST}/${LINUX_MUSL} 
	${CC} ${PROJ} ${CC_FLAGS} ${CC_SELF} --runtime ${LINUX_MUSL} -o ${PUB_DEST}/${LINUX_MUSL}-self-contained --no-build


output_dir:
	if [ ! -d "${ZIP_DEST}" ]; then mkdir ${ZIP_DEST}; fi; 

zip:
	# Windows x64
	zip ${zip_FLAGS} ${ZIP_DEST}/${WIN_RT}.zip ${PUB_DEST}/${WIN_RT}
	zip ${zip_FLAGS} ${ZIP_DEST}/${WIN_RT}-self-contained.zip ${PUB_DEST}/${WIN_RT}-self-contained

	# Windows ARM
	zip ${zip_FLAGS} ${ZIP_DEST}/${WIN_ARM_RT}.zip ${PUB_DEST}/${WIN_ARM_RT}
	zip ${zip_FLAGS} ${ZIP_DEST}/${WIN_ARM_RT}-self-contained.zip ${PUB_DEST}/${WIN_ARM_RT}-self-contained

tar:
	# OSX x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${OSX_RT}.tar -C ${PUB_DEST} ${OSX_RT}
	tar ${tar_FLAGS}  ${ZIP_DEST}/${OSX_RT}-self-contained.tar -C ${PUB_DEST} ${OSX_RT}-self-contained

	# Linux x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_RT}.tar -C ${PUB_DEST} ${LINUX_RT}
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_RT}-self-contained.tar -C ${PUB_DEST} ${LINUX_RT}-self-contained

	# Linux ARM
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_ARM}.tar -C ${PUB_DEST} ${LINUX_ARM}
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_ARM}-self-contained.tar -C ${PUB_DEST} ${LINUX_ARM}-self-contained

	# Linux MUS -x64
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_MUSL}.tar -C ${PUB_DEST} ${LINUX_MUSL}
	tar ${tar_FLAGS}  ${ZIP_DEST}/${LINUX_MUSL}-self-contained.tar -C ${PUB_DEST} ${LINUX_MUSL}-self-contained
