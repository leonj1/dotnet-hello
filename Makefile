# Variables
DOCKER_IMAGE_NAME = weather-api
DOCKER_CONTAINER_NAME = weather-api-container
PORT = 8180

# Colors for pretty output
GREEN = \033[0;32m
NC = \033[0m # No Color

.PHONY: help build run stop clean test logs

help: ## Show this help message
	@echo 'Usage:'
	@echo '  make [target]'
	@echo ''
	@echo 'Targets:'
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  $(GREEN)%-15s$(NC) %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the Docker image
	@echo "Building Docker image..."
	docker build -t $(DOCKER_IMAGE_NAME) WeatherApi/

run: ## Run the container
	@echo "Starting container..."
	docker run -d \
		--name $(DOCKER_CONTAINER_NAME) \
		-p $(PORT):80 \
		$(DOCKER_IMAGE_NAME)
	@echo "Container started on http://localhost:$(PORT)"

stop: ## Stop and remove the container
	@echo "Stopping container..."
	@docker stop -t 0 $(DOCKER_CONTAINER_NAME) 2>/dev/null || true
	@docker rm -f $(DOCKER_CONTAINER_NAME) 2>/dev/null || true
	@echo "Container stopped and removed"

clean: stop ## Stop container, remove container and image
	@echo "Removing image..."
	@docker rmi $(DOCKER_IMAGE_NAME) 2>/dev/null || true
	@echo "Image removed"

test: ## Run the tests
	@echo "Running tests..."
	cd WeatherApi && dotnet test

logs: ## Show container logs
	@docker logs -f $(DOCKER_CONTAINER_NAME)

rebuild: clean build ## Clean and rebuild the Docker image

# Default target
.DEFAULT_GOAL := help
