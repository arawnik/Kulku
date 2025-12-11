#!/bin/sh
set -e

# Replace intermediate env variables with real values
printenv | grep NEXT_PUBLIC_ | while read -r LINE ; do
  KEY=$(echo $LINE | cut -d "=" -f1)
  VALUE=$(echo $LINE | cut -d "=" -f2)

  find .next -type f -exec sed -i "s|STATICENV_${KEY}|${VALUE}|g" {} \;
done

# Execute the container's main process (CMD in Dockerfile)
exec "$@"
