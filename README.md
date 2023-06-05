# How to Run (IIS Express)

From "Visual Studio", simply select the menu Debug -> Start Debugging.

This will run the server app with HTTPS support on a port in the range 44300-44399 *(must be this range for IISExpress to actually enable SSL)*

# How to Build & Run (Docker)

* Build your Docker image by right-clicking on the Dockerfile in the Solution Explorer and selecting "Build Docker Image" or by using the Docker command-line interface.

* Once the Docker image is built, you can run the container and map the host port to the container port using the -p or --publish option of the docker run command. For example, to map port 8888 on the host to port 80 inside the container:

  `docker run -dt -p 8888:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" <image_name>`
  >*(Replace <image_name> with the name or ID of the Docker image you built. ex: docker.io/library/powerplantapi)*
  * "-dt" to run as detached
  * "-p" for port mapping host:container
  * "-e" for environment variables

* After running the docker run command, the Web API will be accessible on the specified host port (8888 in the example).
  >*(ex: http://localhost:8888/swagger/index.html)*
