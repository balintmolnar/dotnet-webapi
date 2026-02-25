pipeline {
	agent any	
	environment {
		ECR_REGISTRY_URL = "957478052151.dkr.ecr.eu-central-1.amazonaws.com"
		APP_NAME = "dotnet-webapi"
		REGION = "eu-central-1"
		APP_VERSION = ""
	}
	parameters {
        choice(
            name: 'ENVIRONMENT',
            choices: ['dev', 'test', 'staging', 'prod'],
            description: 'Environment (dev, test, staging or prod)'
        )
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
					docker.withRegistry("https://${ECR_REGISTRY_URL}", "ecr:${REGION}:aws-jenkins") {
					sh "docker push ${ECR_REGISTRY_URL}/${APP_NAME}:${APP_VERSION}-${BUILD_NUMBER}"
					sh "docker push ${ECR_REGISTRY_URL}/${APP_NAME}:latest"						
					}
				}
			}
		}
		stage('Deploy app to Kubernetes') {
			steps {
				script {
					withAWS(region: 'eu-central-1', credentials: 'aws-jenkins') {
						sh "aws eks update-kubeconfig --name=dotnet-webapi-${params.ENVIRONMENT} --region=${REGION}"
						dir('k8s') {
							sh """helm upgrade --install dotnet-api \
							--values values/values-${params.ENVIRONMENT}.yaml \
							--namespace dotnet-app \
							--set image.tag=${APP_VERSION}-${BUILD_NUMBER} \
							--wait \
							charts/dotnet-webapi
							"""
						}
					}
				}
			}
		}
	}
}