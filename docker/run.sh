if [ "$#" -ne 1 ] || ! [ -d "$1" ]; then
  echo "Usage: $0 REPO_DIRECTORY" >&2
  exit 1
fi
docker run --rm -v $1:/home/ocius/api -it ocius/api_dev 
