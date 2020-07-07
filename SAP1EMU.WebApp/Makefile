

all: clean publish


clean:
	rm -rf ./bin/Desktop/*


publish:
	electronize build /target win
	electronize build /target osx
	electronize build /target linux
