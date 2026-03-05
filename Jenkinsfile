pipeline {
    // Usamos la imagen oficial del SDK de .NET 10 como entorno de ejecución.
    // Montamos el socket de Docker del host para poder construir y publicar imágenes.
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:10.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    // Escucha pushes desde GitHub vía webhook. Requiere el plugin "GitHub" en Jenkins.
    triggers {
        githubPush()
    }

    // Las credenciales se guardan en Jenkins y nunca quedan expuestas en los logs.
    environment {
        DOCKERHUB_USERNAME = credentials('DOCKERHUB_USERNAME')
        DOCKERHUB_TOKEN    = credentials('DOCKERHUB_TOKEN')
        SOLUTION           = 'sabana.devops.users.sln'
        IMAGE_NAME         = "${DOCKERHUB_USERNAME}/sabana-devops-users:latest"
    }

    stages {
        // Descarga el código de la rama que disparó el pipeline.
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        // Descarga los paquetes NuGet antes de compilar.
        stage('Restore dependencies') {
            steps {
                sh 'dotnet restore ${SOLUTION}'
            }
        }

        // Compila en modo Release. --no-restore evita repetir el paso anterior.
        stage('Build') {
            steps {
                sh 'dotnet build ${SOLUTION} --no-restore --configuration Release'
            }
        }

        // Corre las pruebas unitarias. El pipeline se detiene si alguna falla.
        stage('Test') {
            steps {
                sh 'dotnet test ${SOLUTION} --no-build --verbosity normal --configuration Release'
            }
        }

        // Construye y publica la imagen en Docker Hub. Solo corre en rama main.
        stage('Docker Build & Push') {
            when {
                branch 'main'
            }
            steps {
                sh '''
                    echo "$DOCKERHUB_TOKEN" | docker login -u "$DOCKERHUB_USERNAME" --password-stdin
                    docker build -t ${IMAGE_NAME} .
                    docker push ${IMAGE_NAME}
                '''
            }
        }
    }

    post {
        always   { cleanWs() }   // Limpia el workspace al finalizar, pase lo que pase.
        success  { echo '✅ Pipeline completado exitosamente.' }
        failure  { echo '❌ Algo salió mal. Revisa los logs.' }
    }
}