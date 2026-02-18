pipeline {
	agent any
	
	environment {
		ECR_REGISTRY_URL = "957478052151.dkr.ecr.eu-central-1.amazonaws.com/dotnet-webapi"
		APP_NAME = "dotnet-webapi"
		REGION = "eu-central-1"
	}
	stages {
		stage('Versioning') {
			steps {
				script {
					sh "git fetch --tags"
					APP_VERSION = sh(
						script: "git describe --tags --abbrev=0",
						returnStdout: true
						).trim()
					echo "App version: ${APP_VERSION}"
				}
			}
		}
		stage('Build docker image') {
			steps {
				script {
					sh "docker build -t ${ECR_REGISTRY_URL}/${APP_NAME}:${APP_VERSION}-${BUILD_NUMBER} -t ${ECR_REGISTRY_URL}/${APP_NAME}:latest ."
				}
			}
		}
		stage('Push image to ECR') {
			steps {
				script {
					docker.withRegistry("https://${ECR_REGISTRY_URL}", "ecr:${REGION}:aws-balint") {
					sh "docker push ${ECR_REGISTRY_URL}/${APP_NAME}:${APP_VERSION}-${BUILD_NUMBER}"
					sh "docker push ${ECR_REGISTRY_URL}/${APP_NAME}:latest"						
					}
				}
			}
		}
	}
}