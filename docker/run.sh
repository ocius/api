if [ "$#" -ne 1 ] || ! [ -d "$1" ]; then
  echo "Usage: $0 REPO_DIRECTORY" >&2
  exit 1
fi
SHARED_DIRECTORY="$1"
docker run --rm -v $SHARED_DIR:/home/ocius/api -it ocius/api_dev 
