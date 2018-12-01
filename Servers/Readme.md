stop all containers:
docker kill $(docker ps -q)

remove all containers
docker rm $(docker ps -a -q)

remove all docker images
docker rmi $(docker images -q)


# docker-compose -f docker-compose.yml -f docker-compose.override.yml build