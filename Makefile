# Makefile for Weather API

.PHONY: build run stop clean test test-docker test-coverage test-clean logs rebuild help

IMAGE_NAME = weather-api
CONTAINER_NAME = weather-api-container
TEST_IMAGE_NAME = weather-api-test
TEST_CONTAINER_NAME = weather-api-test-container
PORT = 8180

help:
	@echo "Available commands:"
	@echo "  make build         - Build the Docker image"
	@echo "  make run          - Run the container"
	@echo "  make stop         - Stop and remove the container"
	@echo "  make clean        - Remove the image and container"
	@echo "  make test         - Run tests locally"
	@echo "  make test-docker  - Run tests in Docker container"
	@echo "  make test-clean   - Clean up test containers and artifacts"
	@echo "  make logs         - View container logs"
	@echo "  make rebuild      - Clean and rebuild the Docker image"

build:
	@echo "Building Docker image..."
	docker build -t $(IMAGE_NAME) WeatherApi/

run:
	@echo "Starting container..."
	docker run -d \
	--name $(CONTAINER_NAME) \
	-p $(PORT):80 \
	$(IMAGE_NAME)
	@echo "Container started on http://localhost:$(PORT)"

stop:
	@echo "Stopping container..."
	-docker stop $(CONTAINER_NAME)
	-docker rm $(CONTAINER_NAME)
	@echo "Container stopped and removed"

clean: stop
	@echo "Removing image..."
	-docker rmi $(IMAGE_NAME)
	@echo "Image removed"

test: test-docker

test-docker:
	@echo "Building test image..."
	docker build -t $(TEST_IMAGE_NAME) -f Dockerfile.test .
	@echo "Running tests in container..."
	docker run --name $(TEST_CONTAINER_NAME) $(TEST_IMAGE_NAME)
	@echo "Copying test results..."
	docker cp $(TEST_CONTAINER_NAME):/tests/TestResults ./TestResults
	@echo "Test results available in ./TestResults"

test-clean:
	@echo "Cleaning up test artifacts..."
	-docker stop $(TEST_CONTAINER_NAME)
	-docker rm $(TEST_CONTAINER_NAME)
	-docker rmi $(TEST_IMAGE_NAME)
	-rm -rf ./TestResults
	@echo "Test cleanup complete"

logs:
	docker logs -f $(CONTAINER_NAME)

rebuild: clean build
